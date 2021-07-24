using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.DAL.Models;

namespace Forum.DAL.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> UserOwnsPostAsync(string PostId, string UserId);
        Task<IEnumerable<Post>> GetFilteredAndPagedFromThreadAsync(string postName, string threadId, PaginationFilter paginationFilter, string orderByQueryString, string type, string daysAtTown);
        Task<int> GetCountOfFilteredPostsInThreadAsync(string postName, string threadId, string type, string daysAtTown);
        Task<int> GetCountOfAllPostsAsync();
    }
}
