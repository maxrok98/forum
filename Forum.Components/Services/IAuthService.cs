using Forum.Components.Services.Communication;
using Forum.Shared.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Components.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(UserLoginRequest loginModel);
        Task Logout();
        Task<AuthResponse> Register(UserRegistrationRequest registerModel);
        Task<string> RefreshToken();
    }
}
