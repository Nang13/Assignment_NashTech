
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PES.Application.IService;
using PES.Domain.Entities.Model.AggregateChat;
using PES.Presentation.Controllers.V1;
using PES.Presentation.Infrastructures;
using System.Collections.Concurrent;

namespace PES.Presentation.Controllers.v1
{
    public class ChatController : DefaultController
    {

        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IChatService _chatService;
        private readonly ConcurrentDictionary<string, User> _users;
        private readonly IClaimsService _claimServie;


        public ChatController(IHubContext<ChatHub> hubContext,IChatService chatService, ConcurrentDictionary<string, User> users, IClaimsService claimServie)
        {
            _hubContext = hubContext;
            _chatService = chatService;
            _users = users;
            _claimServie = claimServie;
            users["user1"] = new User { Username = "user1", Contacts = new List<string> { "user2" } };
            users["user2"] = new User { Username = "user2", Contacts = new List<string> { "user1" } };
        }

        //[HttpPost]
        //public async Task<IActionResult> SendMessage(ChatMessage message)
        //{
        //    await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.User, message.Message);
        //    await _chatService.SendMessage(message.Message, message.User);
        //    return Ok();
        //}

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var username = "user2";
            if (_users.TryGetValue(username, out var user))
            {
                return Ok(user.Contacts);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddContact([FromBody] string contactUsername)
        {
            /// var username = _claimServie.GetCurrentUserId;
            var username = "user2";
            if (_users.TryGetValue(username, out var user))
            {
                if (!_users.ContainsKey(contactUsername))
                {
                    return NotFound("Contact not found");
                }
                user.Contacts.Add(contactUsername);
                return Ok();
            }
            return NotFound("User not found");
        }

    }
    public class ChatMessage
    {
        public string User { get; set; }
        public string Message { get; set; }
    }




}
