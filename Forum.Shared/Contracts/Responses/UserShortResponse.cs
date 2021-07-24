using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Shared.Contracts.Responses
{
    public class UserShortResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string ImageLink { get; set; }

        public int myPostsAmount { get; set; }
        public int SubscriptionAmount { get; set; }
        public int VotesAmount { get; set; }
    }
}
