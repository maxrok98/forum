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
        Task<IEnumerable<Post>> GetPaged(PaginationFilter paginationFilter, int skip);
        Task<IEnumerable<Post>> GetFilteredAsync(string postName);
        Task<IEnumerable<Post>> GetFromThreadAsync(string threadId);
        Task<IEnumerable<Post>> GetPagedFromThreadAsync(string threadId, PaginationFilter paginationFilter, int skip);
        Task<IEnumerable<Post>> GetFilteredAndPagedAsync(string postName, PaginationFilter paginationFilter, int skip);
        Task<IEnumerable<Post>> GetFilteredFromThreadAsync(string postName, string threadId);
        Task<IEnumerable<Post>> GetFilteredAndPagedFromThreadAsync(string postName, string threadId, PaginationFilter paginationFilter, int skip);
        Task<int> GetCountOfFilteredPostsAsync(string postName);
        Task<int> GetCountOfPostsInThreadAsync(string threadId);
        Task<int> GetCountOfFilteredPostsInThreadAsync(string postName, string threadId);
        Task<int> GetCountOfAllPostsAsync();
    }
}
