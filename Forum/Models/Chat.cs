using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Models
{
    public class Chat
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public string AddedId { get; set; }
        public virtual User Added { get; set; }

        public bool Encrypted { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
