using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Services.Communication;
using Forum.DAL.Models;

namespace Forum.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<UsersResponse> GetAllAsync(string userName, PaginationFilter paginationFilter);
        Task<User> GetAsync(string id);
        Task<UserResponse> UpdatePasswordAsync(string id, string currentPassword, string newPassword);
        Task<UserResponse> UpdateImageAsync(string id, byte[] image);
        Task<UserResponse> UpdateAccountAsync(string id, string email, string userName);
        Task<UserResponse> RemoveAsync(string id);
        Task<bool> IsUserAdmin(string id);
    }
}
