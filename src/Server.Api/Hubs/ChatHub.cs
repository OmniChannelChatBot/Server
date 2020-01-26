using Microsoft.AspNetCore.SignalR;
using Server.Api.Models;
using System;
using System.Threading.Tasks;

namespace Server.Api.Hubs
{
    public class ChatHub : Hub
    {
        public Task NewMessage(ChatMessage message) =>
            Clients.All.SendAsync("MessageReceived", message, Context.ConnectionId);

        public override async Task OnConnectedAsync()
        {
            var context = this.Context.GetHttpContext();
            // получаем кук name
            if (context.Request.Cookies.ContainsKey("name"))
            {
                if (context.Request.Cookies.TryGetValue("name", out string userName))
                {
                    //Debug.WriteLine($"name = {userName}");
                }
            }
            //// получаем юзер-агент
            //Debug.WriteLine($"UserAgent = {context.Request.Headers["User-Agent"]}");
            //// получаем ip
            //Debug.WriteLine($"RemoteIpAddress = {context.Connection.RemoteIpAddress.ToString()}");

            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
