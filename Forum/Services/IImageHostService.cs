using Forum.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Services
{
    public interface IImageHostService
    {
        Task<ImageHostResponse> SaveImageAsync(string base64string);
    }
}
