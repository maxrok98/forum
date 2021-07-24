using Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services.Communication
{
    public class MessageResponse : BaseResponse<Message>
    {
        public MessageResponse(Message message) : base(message) { }
        public MessageResponse(string message) : base(message) { }
    }
}
