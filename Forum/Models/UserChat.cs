using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class UserChat
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public string ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
