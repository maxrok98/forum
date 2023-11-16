using Forum.Shared.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Client.Services
{
    public interface IUserService
    {
        Task<PageResponse<UserShortResponse>> GetAll(string page, string pageSize, string searchTerm);
        Task<bool> DeleteAsync(string id);
    }
}
