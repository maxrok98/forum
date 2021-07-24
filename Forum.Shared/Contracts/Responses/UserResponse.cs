using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Shared.Contracts.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string ImageLink { get; set; }

        public virtual List<PostForUserResponse> myPosts { get; set; }
        public virtual List<ThreadResponse> Subscription { get; set; }
        public virtual List<PostForUserResponse> Votes { get; set; }
        public virtual List<PostForUserResponse> Calendar { get; set; }
    }
}
