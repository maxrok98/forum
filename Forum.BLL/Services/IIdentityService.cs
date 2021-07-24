using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.BLL.Services.Communication;

namespace Forum.BLL.Services
{
    public interface IIdentityService
    {
        Task<AuthentificationResult> RegisterAsync(string email, string password, byte[] image, string UserName);

        Task<AuthentificationResult> LoginAsync(string email, string password);

        Task<AuthentificationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
