using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services.Communication
{
    public class PostsResponse : BaseResponse<IEnumerable<Post>>
    {
        public int amountOfPosts { get; set; }
        public PostsResponse(IEnumerable<Post> post, int amountOfPosts) : base(post) {
            this.amountOfPosts = amountOfPosts;
        }
        public PostsResponse(string message) : base(message) { }
    }
}
