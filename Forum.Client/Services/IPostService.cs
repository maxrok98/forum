using Forum.Shared.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Client.Services
{
    interface IPostService
    {
        Task<PageResponse<PostResponse>> loadPosts(string Page, string PageSize, string ThreadId, string SearchTerm, string OrderBy, string Type, string DaysAtTown);
    }
}
