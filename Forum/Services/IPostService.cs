using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Services.Communication;
using Forum.Contracts;

namespace Forum.Services
{
    public interface IPostService
    {
        Task<PostsResponse> GetAllAsync(string postName = null, string threadId = null, PaginationFilter paginationFilter = null, string orderByQueryString = null);
        Task<PostResponse> GetAsync(string id);
        Task<int> GetCountOfAllPostsAsync();
        Task<PostResponse> AddAsync(Post post);
        Task<PostResponse> UpdateAsync(string id, Post post);
        Task<PostResponse> RemoveAsync(string id);
        Task<VoteResponse> Vote(string PostId, string UserId);
        Task<VoteResponse> UnVote(string PostId, string UserId);
        Task<bool> UserOwnsPostAsync(string PostId, string UserId);
    }
}
