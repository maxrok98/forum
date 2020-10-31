using Forum.Client.Services.Communication;
using Forum.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Client.Services
{
    interface IAuthService
    {
        Task<AuthResponse> Login(UserLoginRequest loginModel);
        Task Logout();
        Task<AuthResponse> Register(UserRegistrationRequest registerModel);
    }
}
