using Microsoft.Extensions.Options;
using Server.Helper;
using Server.Model;
using Server.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Service
{
    public interface IChatRoomService
    {
        Task<List<ChatRoom>> GetChatRoomsAsync();
        Task<List<ChatRoom>> GetChatRoomsAsync(int userId);
        Task<bool> AddChatRoomAsync(ChatRoom newChatRoom);
    }

    public class ChatRoomService : IChatRoomService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DBApiSettings _dbApiSettings;

        public ChatRoomService(IHttpClientFactory httpClientFactory,
            IOptions<DBApiSettings> dbApiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _dbApiSettings = dbApiSettings.Value;
        }

        public async Task<List<ChatRoom>> GetChatRoomsAsync(int userId)
        {
            var chatRooms = new List<ChatRoom>();

            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync(_dbApiSettings.Url + "/" + userId);

                chatRooms = JsonSerializerHelper.Deserialize<List<ChatRoom>>(response);
            }

            return chatRooms;
        }

        public async Task<List<ChatRoom>> GetChatRoomsAsync()
        {
            var chatRooms = new List<ChatRoom>();

            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync(_dbApiSettings.Url);

                chatRooms = JsonSerializerHelper.Deserialize<List<ChatRoom>>(response);
            }

            return chatRooms;
        }

        public async Task<bool> AddChatRoomAsync(ChatRoom chatRoom)
        {
            bool success;

            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.PostAsync(_dbApiSettings.Url + "/create",
                    JsonSerializerHelper.Serialize(chatRoom));

                success = JsonSerializerHelper.Deserialize<bool>(response);
            }

            return success;
        }
    }
}
