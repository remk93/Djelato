﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Djelato.Common.Shared;
using Djelato.DataAccess.RedisRepositories.Interfaces;
using Djelato.Services.Models;
using Djelato.Services.Notification;
using Djelato.Services.Services.Interfaces;
using Djelato.Web.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Djelato.Web.Controllers
{
    [ApiController]
    [EnableCors("CorsPolicy")]    
    [Route("api/[controller]")]
    public class UserController : BaseWebApiController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly INotifier _emailNotifier;
        private readonly IRedisRepo _redis;
        private readonly IFileService _fileService;

        public UserController(ILogger<UserController> logger, 
            IMapper mapper, 
            IUserService userService, 
            INotifier emailNotifier, 
            IRedisRepo redis,
            IFileService fileService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _emailNotifier = emailNotifier;
            _redis = redis;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] UserDTO user)
         {
            try
            {
                bool isExist = await _userService.CheckByEmailAsync(user.Email.ToLower());
                if (isExist)
                {
                    _logger.LogInformation($"User {user.Name} use email which exist in database");
                    var result = ClientError("This email are exist");
                    return result;
                }

                var imgRouteRes = await _fileService.SaveAvatarImg(user.Avatar);
                if (!imgRouteRes.IsSuccessful || string.IsNullOrEmpty(imgRouteRes.Obj))
                {
                    _logger.LogInformation($"User avatar wasn't saved in root directory. (User {user.Name})");
                    var result = ClientError(imgRouteRes.Message);
                    return result;
                }

                UserModel model = _mapper.Map<UserModel>(user); // configure email to lower case in mapper settings!
                model.AvatarPath = imgRouteRes.Obj;

                ServiceResult addResult = await _userService.AddAsync(model);
                if (!addResult.IsSuccessful)
                {
                    var result = ServerError();
                    return result;
                }

                int maxRandom = 1000000;
                int minRandom = 1;

                var key = RandomGenerator.RandomNumber(minRandom, maxRandom);
                await _emailNotifier.SendKey(user.Email.ToLower(), key);

                bool isCache = await _redis.SetAsync(key.ToString(), user.Email.ToLower());
                if (isCache)
                {
                    var result = Success(null, "Profile created");
                    return result;
                }
                else
                {
                    var result = ServerError();
                    return result;
                }
            }
            catch (MongoWriteException mwEx)
            {
                if (mwEx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    _logger.LogInformation($"User: -{user.Name}- tried to register an email which already exist");
                    var result = ClientError("This email are exist");
                    return result;
                }
                else
                {
                    var result = ServerError();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"When user: -{user.Name}- tried to create new profile, appeared the exception: {ex.Source}");
                _logger.LogError($"Code message for this error: {ex.Message}");
                _logger.LogTrace($"Trace for error: {ex.StackTrace}");

                var result = ServerError();
                return result;
            }
        }

        [HttpPost]
        [Route("confirmEmail/{key}")]
        public async Task<IActionResult> ConfirmEmailAsync(string key)
        {
            try
            {
                Match matchKey = Regex.Match(key, RegexExpressions.KeyRgx, RegexOptions.IgnoreCase);
                if (!matchKey.Success)
                {
                    _logger.LogInformation($"confirm key is not the Guid type");
                    var result = ClientError("Email didn't confirm. You use uncorrect key!");
                    return result;
                }

                var email = await _redis.GetAsync(key);
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogError($"Redis return empty or null eamil value");
                    _logger.LogTrace($"Trace for error: Djelato.Web.Controllers.CondirmEmailAsync()");

                    var result = ClientError("Email didn't confirm. You use uncorrect key!");
                    return result;
                }

                bool isConfirmed = await _userService.ConfirmEmailAsync(email);

                if (isConfirmed)
                {
                    var result = Success(null, "Your email was confirmed. Welcome to our ice-cream family!");
                    return result;
                }
                else
                {
                    var result = ServerError();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"When user tried confim an eamil, appeared the exception: {ex.Source}");
                _logger.LogInformation($"Code message for this error: {ex.Message}");
                _logger.LogTrace($"Trace for error: {ex.StackTrace}");

                var result = ServerError();
                return result;
            }
        }
    }
}