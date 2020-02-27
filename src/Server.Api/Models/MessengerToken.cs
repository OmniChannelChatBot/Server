using System.Collections.Generic;

namespace Server.Api.Models
{
    public class MessengerToken
    {
        public string Token { get; set; }

        /// <summary>
        /// List of groups in account
        /// </summary>
        public List<string> Groups { get; set; }
    }
}
