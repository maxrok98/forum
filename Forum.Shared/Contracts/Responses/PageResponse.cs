using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Contracts.Responses
{
    public class PageResponse<T>
    {
        public PageResponse(){}

        public PageResponse(IEnumerable<T> data)
        {
            Results = data;
        }

        public IEnumerable<T> Results { get; set; }
        public int? PageCount { get; set; }
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
    }
}
