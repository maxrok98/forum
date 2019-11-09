using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class ThreadImage
    {
        public string Id { get; set; }
        public byte[] Image { get; set; }

        public string ThreadId { get; set; }
        public Thread Thread { get; set; }
    }
}
