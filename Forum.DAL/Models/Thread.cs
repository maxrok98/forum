using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DAL.Models
{
    public class Thread
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string ImageLink { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
