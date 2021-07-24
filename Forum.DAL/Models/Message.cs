using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DAL.Models
{
    public class Message
    {
        public string Id { get; set; }

        public string ChatId { get; set; }
        public virtual Chat Chat { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
