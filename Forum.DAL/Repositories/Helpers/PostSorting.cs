using Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DAL.Repositories.Helpers
{
    public class PostSorting : SortingHelper<Post>
    {
        public PostSorting()
        {
            defaultQuery = "date descending";
        }
    }
}
