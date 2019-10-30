using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.DTOout
{
    public class ComentDTOout
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
