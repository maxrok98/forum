using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Services.Communication;

namespace Forum.Services
{
    public interface IIdentityService
    {
        Task<AuthentificationResult> RegisterAsync(string email, string password);

        Task<AuthentificationResult> LoginAsync(string email, string password);

        Task<AuthentificationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
