using Forum.BLL.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.BLL.Services
{
    public interface IImageHostService
    {
        Task<ImageHostResponse> SaveImageAsync(string base64string);
    }
}
