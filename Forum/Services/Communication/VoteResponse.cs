using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.DAL.Models;

namespace Forum.Services.Communication
{
    public class VoteResponse : BaseResponse<Vote>
    {
        public VoteResponse(Vote vote) : base(vote) { }
        public VoteResponse(string message) : base(message) { }
    }
}
