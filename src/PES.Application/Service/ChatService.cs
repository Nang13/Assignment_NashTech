using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using PES.Application.IService;
using PES.Domain.Entities.Model.AggregateChat;
using PES.Infrastructure.Data.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Application.Service
{
    public class ChatService : IChatService
    {
        private readonly IMongoCollection<Message> _messageCollections;

        public ChatService(IOptions<MessagesStorageDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
         bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);
            _messageCollections = mongoDatabase.GetCollection<Message>(
                bookStoreDatabaseSettings.Value.MessagesCollectionName);
        }
        public Task<List<Message>> GetMessages()
        {
            throw new NotImplementedException();
        }

        public async Task SendMessage(string content, string senderId)
        {
            var message = new Message
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Text = content,
                SenderId = senderId,
                Timestamp = DateTime.UtcNow,
                GroupId = ObjectId.GenerateNewId().ToString()
            };
            await _messageCollections.InsertOneAsync(message);
        }
    }
}
