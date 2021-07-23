using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Contracts.Responses
{
    public class MessageResponse
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public UserShortResponse User { get; set; }
        public DateTime Date { get; set; }
    }
}
