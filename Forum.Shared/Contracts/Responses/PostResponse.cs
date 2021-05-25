using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Contracts.Responses
{
    public class PostResponse
    {
        public string Id { get; set; }
        public string ThreadId { get; set; }
        //public Thread Thread { get; set; }
        public string ThreadName { get; set; }

        public string UserId { get; set; }
        //public User User { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }

        public string ImageLink { get; set; }

        public DateTime Date { get; set; }
        public DateTime DateOfEvent { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PostType PostType { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public int Rating { get; set; }
        public virtual ICollection<ComentResponse> Coments { get; set; }
    }
}
