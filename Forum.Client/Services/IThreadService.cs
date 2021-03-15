using Forum.Client.Services.Communication;
using Forum.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Client.Services
{
    interface IThreadService
    {
        Task<List<ThreadResponse>> GetAll();
        Task<ServiceResponse> DeleteThread(string id);
        Task<ServiceResponse> SubscribeToThread(string ThreadId);
        Task<ThreadResponse> Get(string id);
    }
}
