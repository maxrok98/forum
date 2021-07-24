using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.BLL.Services.Communication
{
    public class ImageHostResponse
    {
        public Data data { get; set; }

        public bool success { get; set; }
        public int status { get; set; }
    }

    public class Data
    {
        public string display_url { get; set; }
    }
}
