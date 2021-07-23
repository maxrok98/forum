using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class User : IdentityUser
    {
        //public string Id { get; set; }
        public int Year { get; set; }
        public string PublicKey { get; set; }

        public string ImageLink { get; set; }

        public virtual ICollection<UserChat> Chats { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Coment> Coments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Calendar> Calendar { get; set; }

        public virtual RefreshToken RefreshToken { get; set; }

    }
}
