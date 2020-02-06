using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Server.Api.Models;
using System;
using System.Threading.Tasks;

namespace Server.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public Task NewChatMessageAsync(ChatMessage chatMessage)
        {
            var username = this.Context.UserIdentifier;
            return Clients.Others.SendAsync("ChatMessageReceived", username, chatMessage);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Disconnected", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
