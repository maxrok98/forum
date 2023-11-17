using Forum.Components.Services.Communication;
using Forum.Shared.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Components.Services
{
    public interface IThreadService
    {
        Task<List<ThreadResponse>> GetAll();
        Task<ServiceResponse> DeleteThread(string id);
        Task<ServiceResponse> SubscribeToThread(string ThreadId);
        Task<ThreadResponse> Get(string id);
    }
}
