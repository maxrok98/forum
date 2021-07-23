using Forum.Models;
using Forum.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services
{
    public interface IChatService
    {
        Task<ChatsResponse> GetAllAsync();
        Task<ChatsResponse> GetAllFromUserAsync(string id);
        Task<ChatResponse> GetAsync(string id, string userId);
        Task<ChatResponse> AddAsync(Chat chat);
        Task<ChatResponse> UpdateAsync(string id, string userId, Chat chat);
        Task<ChatResponse> RemoveAsync(string id, string userId);
    }
}
