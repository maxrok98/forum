using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Shared.Contracts.Responses
{
    public class ThreadResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string ImageLink { get; set; }

        public int NumberOfSubscription { get; set; }
        public int NumberOfPost { get; set; }
    }
}
