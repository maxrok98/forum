using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Shared.Contracts.Responses
{
    public class ChatResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<MessageResponse> Messages { get; set; }
        public ICollection<UserShortResponse> Users { get; set; }
    }
}
