using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Models
{
    public class Vote
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
