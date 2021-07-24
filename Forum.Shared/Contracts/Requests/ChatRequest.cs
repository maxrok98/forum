using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Forum.Shared.Contracts.Requests
{
    public class ChatRequest
    {
        [Required]
        public string Name { get; set; }

        public ICollection<UserRequest> Users { get; set; }
    }
}
