using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetOrderByVoteAsync();
        Task<IEnumerable<Post>> GetOrderByDateAsync();
        Task<Post> UserOwnsPostAsync(string PostId, string UserId);
        Task<IEnumerable<Post>> GetFilteredAndPagedFromThreadAsync(string postName, string threadId, PaginationFilter paginationFilter, string orderByQueryString);
        Task<int> GetCountOfFilteredPostsInThreadAsync(string postName, string threadId);
        Task<int> GetCountOfAllPostsAsync();
    }
}
