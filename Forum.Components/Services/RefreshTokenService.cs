using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Components.Services
{
    public class RefreshTokenService
    {
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IAuthService _authService;
        private readonly ILocalStorageService _localStorage;
        public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthService authService, ILocalStorageService localStorage)
        {
            _authProvider = authProvider;
            _authService = authService;
            _localStorage = localStorage;
        }
        public async Task<string> TryRefreshToken()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                var user = authState.User;
                var exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
                var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
                var timeUTC = DateTime.UtcNow;
                var diff = expTime - timeUTC;
                if (diff.TotalMinutes <= 0)
                    return await _authService.RefreshToken();
            }
            return await _localStorage.GetItemAsStringAsync("JWT-Token");
        }
    }
}
