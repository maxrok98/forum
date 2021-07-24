using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.DAL.Models;

namespace Forum.BLL.Services.Communication
{
    public class SubscriptionResponse : BaseResponse<Subscription>
    {
        public SubscriptionResponse(Subscription sub) : base(sub) { }
        public SubscriptionResponse(string message) : base(message) { }
    }
}
