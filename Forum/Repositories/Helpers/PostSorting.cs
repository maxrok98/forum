using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Repositories.Helpers
{
    public class PostSorting : SortingHelper<Post>
    {
        public PostSorting()
        {
            base.defaultQuery = "date descending";
        }
    }
}
