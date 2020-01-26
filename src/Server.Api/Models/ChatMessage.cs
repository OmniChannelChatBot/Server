using Server.Api.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Api.Models
{
    public class ChatMessage
    {
        [Required]
        public int ChatRoomId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Text { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ChatMessageType Type { get; set; }
    }
}
