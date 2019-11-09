using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class UserImage
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
