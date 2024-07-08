using PES.Domain.Entities.Model.AggregateChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Application.IService
{
    public interface IChatService
    {
        public Task<List<Message>> GetMessages();

        public Task SendMessage(string content, string senderId);

        
    }
}
