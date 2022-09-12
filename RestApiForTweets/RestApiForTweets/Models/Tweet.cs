﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestApiForTweets.Models
{
    [BsonIgnoreExtraElements]
    public class Tweet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Sender { get; set; }
        public string? Tag { get; set; }
        public string? Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}
