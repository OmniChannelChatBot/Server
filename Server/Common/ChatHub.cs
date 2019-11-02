using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Common
{
    /// <summary>
    /// OmniChannel ChatHub.
    /// </summary>
    //[Authorize]
#pragma warning disable CA1034 // Nested types should not be visible
    public class ChatHub : Hub
#pragma warning restore CA1034 // Nested types should not be visible
    {
        public ChatHub()
        {
        }

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="message">сообщение.</param>
        /// <param name="userName">кому сообщение.</param>
        /// <returns>Task.</returns>
        //public async Task Send(string message, string userName)
        //{
        //    await Clients.All.SendAsync("Send", message, userName);
        //}

        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Receive", message, Context.ConnectionId);
        }

        public override async Task OnConnectedAsync()
        {
            var context = this.Context.GetHttpContext();
            // получаем кук name
            if (context.Request.Cookies.ContainsKey("name"))
            {
                string userName;
                if (context.Request.Cookies.TryGetValue("name", out userName))
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
