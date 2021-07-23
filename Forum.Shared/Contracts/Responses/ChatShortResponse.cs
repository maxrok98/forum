using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Contracts.Responses
{
    public class ChatShortResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserShortResponse> Users { get; set; } 
        public MessageResponse LastMessage { get; set; }
    }
}
