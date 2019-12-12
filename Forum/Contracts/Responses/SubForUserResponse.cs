using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Contracts.Responses
{
    public class SubForUserResponse
    {
       public string Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string ThreadId { get; set; }
        public virtual Thread Thread { get; set; }
    }
}
