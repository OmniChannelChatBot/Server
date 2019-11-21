using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class ChatRoom
    {
        public Guid Guid { get; set; }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTimeOffset Date { get; set; }

        //public List<User> Users { get; set; }
    }
}
