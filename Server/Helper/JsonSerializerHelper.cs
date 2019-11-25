using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Server.Helper
{
    public static class JsonSerializerHelper
    {
        public static StringContent Serialize<TValue>(TValue value)
        {
            return new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        public static TValue Deserialize<TValue>(HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<TValue>
                    (response.Content.ReadAsStringAsync().Result, options);
        }
    }
}
