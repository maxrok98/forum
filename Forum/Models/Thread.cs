using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Thread
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string ImageId { get; set; }
        public virtual ThreadImage Image { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
