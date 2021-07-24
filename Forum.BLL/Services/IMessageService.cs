using Forum.BLL.Services.Communication;
using Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.BLL.Services
{
    public interface IMessageService
    {
        Task<MessageResponse> AddAsync(Message message);
    }
}
