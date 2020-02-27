using System.Text.Json.Serialization;

namespace Server.Api.Models.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessangerType
    {
        OmniChannelChatBot = 1,
        VK = 2,
        WhatsApp = 3,
        Telegram = 4
    }
}