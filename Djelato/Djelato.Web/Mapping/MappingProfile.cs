﻿using AutoMapper;
using Djelato.DataAccess.Entity;
using Djelato.Services.Models;
using Djelato.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Djelato.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User region

            CreateMap<UserDTO, UserModel>()
                .ForMember(x => x.Name, x => x.MapFrom(src => src.Name))
                .ForMember(x => x.Email, x => x.MapFrom(src => src.Email.ToLower().Trim())) // save email in database in lower case (send lowercase email to services)
                .ForMember(x => x.Role, x => x.MapFrom(src => src.Role))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(src => src.PhoneNumber))
                .ForMember(x => x.Password, x => x.MapFrom(src => src.Password))
                .ForMember(x => x.EmailConfirmed, x => x.MapFrom(src => src.EmailConfirmed));

            CreateMap<User, UserModel>()
                .ForMember(x => x.Name, x => x.MapFrom(src => src.Name))
                .ForMember(x => x.Email, x => x.MapFrom(src => src.Email))
                .ForMember(x => x.Role, x => x.MapFrom(src => src.Role))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(src => src.PhoneNumber));

            CreateMap<UserModel, User>()
               .ForMember(x => x.Name, x => x.MapFrom(src => src.Name))
               .ForMember(x => x.Email, x => x.MapFrom(src => src.Email))
               .ForMember(x => x.Role, x => x.MapFrom(src => src.Role))
               .ForMember(x => x.PhoneNumber, x => x.MapFrom(src => src.PhoneNumber));

            #endregion

            #region Auth model

            CreateMap<AuthDTO, AuthModel>()
                .ForMember(x => x.Email, x => x.MapFrom(src => src.Email))
                .ForMember(x => x.Password, x => x.MapFrom(src => src.Password));

            CreateMap<AuthModel, AuthDTO>()
                .ForMember(x => x.Email, x => x.MapFrom(src => src.Email))
                .ForMember(x => x.Password, x => x.MapFrom(src => src.Password));

            #endregion
        }
    }
}
