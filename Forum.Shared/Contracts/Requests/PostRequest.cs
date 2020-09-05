using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Contracts.Requests
{
    public class PostRequest
    {
        [Required]
        public string ThreadId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
    
        public byte[] Image { get; set; }
    }
}
