using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Services.Communication
{
    public class ThreadResponse : BaseResponse<Thread>
    {
        public ThreadResponse(Thread thread) : base(thread) { }
        public ThreadResponse(string message) : base(message) { }
    }
}
