using Server.Api.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Server.Api.Models
{
    public class ChatMessage
    {
        [Required]
        public int ChatRoomId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public ChatMessageType Type { get; set; }
    }
}
