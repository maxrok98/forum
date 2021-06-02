using Forum.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Client.Services
{
    interface IUserService
    {
        Task<PageResponse<UserShortResponse>> GetAll(string page, string pageSize, string searchTerm);
        Task<bool> DeleteAsync(string id);
    }
}
