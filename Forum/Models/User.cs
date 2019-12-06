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
        public int Year { get; set; }
        public string PublicKey { get; set; }

        public string ImageId { get; set; }
        public virtual UserImage Image { get; set; }

        //public string RefreshTokenId { get; set; }
        //public virtual RefreshToken RefreshToken { get; set; }

        public virtual ICollection<Chat> MyChats { get; set; }
        public virtual ICollection<Chat> OtherChats { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Coment> Coments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

    }
}
