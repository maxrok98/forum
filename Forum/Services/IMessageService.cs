using Forum.Models;
using Forum.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services
{
    public interface IMessageService
    {
        Task<MessageResponse> AddAsync(Message message);
    }
}
