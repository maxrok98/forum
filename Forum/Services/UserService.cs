using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Repositories;
using Forum.Services.Communication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users.Include(x => x.Posts).Include(x => x.Subscriptions).ThenInclude(x => x.Thread).ToListAsync();
        }

        public async Task<UsersResponse> GetAllAsync(string userName, PaginationFilter paginationFilter)
        {
            IQueryable<User> query = _userManager.Users.OrderBy(x => x.UserName).Include(x => x.Image).Include(x => x.Posts).Include(x => x.Subscriptions).ThenInclude(x => x.Thread).Include(x => x.Votes).ThenInclude(x => x.Post);
            if (!string.IsNullOrEmpty(userName))
                query = query.Where(u => u.UserName.Contains(userName));
            if (paginationFilter != null)
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                query = query.Skip(skip).Take(paginationFilter.PageSize);
            }
            return new UsersResponse(await query.ToListAsync(), _userManager.Users.Count());
        }

        public async Task<User> GetAsync(string id)
        {
            return await _userManager.Users.Where(x => x.Id == id).Include(x => x.Posts).Include(x => x.Subscriptions).ThenInclude(x => x.Thread).Include(x => x.Image).Include(x => x.Votes).ThenInclude(x => x.Post).FirstAsync();
        }

        public async Task<bool> IsUserAdmin(string id)
        {
            var user = await _userManager.Users.Where(x => x.Id == id).FirstAsync();
            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                if (role == "Admin")
                    return true;
            }
            return false;
        }

        public async Task<UserResponse> RemoveAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
                return new UserResponse("User not found.");

            try
            {
                await _userManager.DeleteAsync(existingUser);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(existingUser);
            }
            catch (Exception ex)
            {
                return new UserResponse($"An error occurred when deleting the user: {ex.Message}");
            }
        }

        public async Task<UserResponse> UpdatePasswordAsync(string id, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return new UserResponse("User not found.");
            try
            {
                await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                await _unitOfWork.CompleteAsync();

                return new UserResponse(user);
            }
            catch (Exception ex)
            {
                return new UserResponse($"An error occurred when changing password: {ex.Message}");
            }
        }
    }
}
