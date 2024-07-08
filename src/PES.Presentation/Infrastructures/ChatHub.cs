using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace PES.Presentation.Infrastructures
{

    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();


        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                UserConnections[username] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                UserConnections.TryRemove(username, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        //? one-to-one
        //public async Task SendPrivateMessage(string user, string message)
        //{


        //    //? send 
        //    //await Clients.All.SendAsync("ReceivePrivateMessage", Context.User.Identity.Name, message);
        //}

        // Group chat
        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        public async Task SendPrivateMessage(string recipientUsername, string message)
        {
            UserConnections.TryAdd("user1", "user2");
            UserConnections.TryAdd("user2", "user1");
            var senderUsername = "user1";
            if (string.IsNullOrEmpty(senderUsername))
            {
                throw new HubException("Sender is not authenticated.");
            }

            if (UserConnections.TryGetValue(recipientUsername, out var recipientConnectionId))
            {
                await Clients.Client(recipientConnectionId).SendAsync("ReceivePrivateMessage", senderUsername, message);

                await Clients.Client(recipientConnectionId).SendAsync("ReceivePrivateMessage", senderUsername, message);
            }
            else
            {
                await Clients.Client("1111111").SendAsync("ReceivePrivateMessage", senderUsername, message);
                throw new HubException("Recipient is not connected.");
            }
        }

        // Join a group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.User.Identity.Name} has joined the group {groupName}.");
        }

        // Leave a group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.User.Identity.Name} has left the group {groupName}.");
        }
    }
}
