using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Contracts.Responses
{
    public class ComentResponse
    {
        public string Id { get; set; }
        public string ParentComentId { get; set; }

        public string UserId { get; set; }
        public string  UserName { get; set; }

        public string Text { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<ComentResponse> SubComents { get; set; }
    }
}
