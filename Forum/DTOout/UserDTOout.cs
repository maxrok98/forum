using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.DTOout
{
    public class UserDTOout
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string ImageId { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<PostForUserDTOout> myPosts { get; set; }
        public virtual ICollection<ThreadDTOout> myThread { get; set; }
    }
}
