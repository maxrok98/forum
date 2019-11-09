using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class PostImage
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }

        public string PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
