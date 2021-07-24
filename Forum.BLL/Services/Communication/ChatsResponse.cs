using Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.BLL.Services.Communication
{
    public class ChatsResponse : BaseResponse<IEnumerable<Chat>>
    {
        public ChatsResponse(IEnumerable<Chat> chats) : base(chats) { }
        public ChatsResponse(string message) : base(message) { }
    }
}
