﻿using Djelato.DataAccess.Context;
using Djelato.DataAccess.Entity;
using Djelato.DataAccess.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Djelato.DataAccess.Repository
{
    internal class UserRepository<T> : IUserRepository<T> where T : class
    {
        private readonly MongoContext<T> _context = null;

        internal UserRepository(MongoContext<T> context)
        {
            _context = context;
        }

        public async Task AddAsync(T obj)
        {
            await _context.Collection.InsertOneAsync(obj);
        }

        public async Task<bool> CheckAsync(string email)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("Email", email);
            long documentNumber = await _context.Collection.CountDocumentsAsync(filter);

            if (documentNumber == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<T> GetAsync(string email)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("Email", email);
            T obj = await _context.Collection.Find(filter).FirstOrDefaultAsync();
            return obj;
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync(string id, T obj)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("Id", id);
            ReplaceOneResult replaceResult = await _context.Collection.ReplaceOneAsync(filter, replacement: obj);
            return replaceResult;
        }
    }
}
