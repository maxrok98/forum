using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.DTOin
{
    public class PostDTOin
    {
        public string ThreadId { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }
    
        public byte[] Image { get; set; }
    }
}
