﻿using Djelato.Common.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Djelato.DataAccess.Entity
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("AvatarPath")]
        public string AvatarPath { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [BsonElement("Role")]
        public Role Role { get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("Salt")]
        public byte[] Salt { get; set; }

        [BsonElement("Hash")]
        public string PasswordHash { get; set; }        
    }
}
