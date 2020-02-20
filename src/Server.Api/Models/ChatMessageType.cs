using System.Text.Json.Serialization;

namespace Server.Api.Models.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChatMessageType
    {
        Text = 1,
        File = 2,
        Voice = 3
    }
}
