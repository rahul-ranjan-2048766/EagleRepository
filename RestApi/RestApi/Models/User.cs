﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestApi.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? LoginId { get; set; }
        public string? Password { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime DateTime { get; set; }
    }
}
