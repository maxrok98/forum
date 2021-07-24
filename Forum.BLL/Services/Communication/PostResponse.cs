using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.DAL.Models;

namespace Forum.BLL.Services.Communication
{
    public class PostResponse : BaseResponse<Post>
    {
        public PostResponse(Post post) : base(post) { }
        public PostResponse(string message) : base(message) { }
    }
}
