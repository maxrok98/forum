using Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services.Communication
{
    public class ChatResponse : BaseResponse<Chat>
    {
        public ChatResponse(Chat chat) : base(chat) { }
        public ChatResponse(string message) : base(message) { }
    }
}
