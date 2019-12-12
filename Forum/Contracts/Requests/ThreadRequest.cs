using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Contracts.Requests
{
    public class ThreadRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}
