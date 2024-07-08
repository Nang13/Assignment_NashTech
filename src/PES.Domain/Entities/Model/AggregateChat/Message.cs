using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Domain.Entities.Model.AggregateChat
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ReceiverId { get; set; }  // Used for one-on-one messages

        [BsonRepresentation(BsonType.ObjectId)]
        public string GroupId { get; set; }  // Used for group messages

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
