using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DAL.Models
{
    public class Event : Post
    {
        public DateTime DateOfEvent { get; set; }
        public virtual ICollection<Calendar> Calendar { get; set; }
    }
}
