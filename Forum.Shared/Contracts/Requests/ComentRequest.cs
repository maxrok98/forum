using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Contracts.Requests
{
    public class ComentRequest
    {
        public string PostId { get; set; }

        public string ParentComentId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
