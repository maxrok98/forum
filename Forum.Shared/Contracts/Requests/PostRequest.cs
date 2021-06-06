using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Contracts.Requests
{
    public class PostRequest
    {
        [Required]
        public string ThreadId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Content { get; set; }
    
        public byte[] Image { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfEvent { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PostType PostType { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
