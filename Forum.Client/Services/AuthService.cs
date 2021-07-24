using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http.Json;
using Forum.Client.Services.Communication;
using Forum.Shared.Contracts.Requests;
using Forum.Shared.Contracts.Responses;
using Forum.Shared.Contracts;

namespace Forum.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory httpClientFactory,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClientFactory.CreateClient("AuthHttpClient");
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthResponse> Register(UserRegistrationRequest registerModel)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Identity.Register, registerModel);
            if (response.IsSuccessStatusCode)
            {
                AuthSuccessResponse cont = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();
                await _localStorage.SetItemAsync<string>("JWT-Token", cont.Token);
                await _localStorage.SetItemAsync<string>("JWT-RefreshToken", cont.RefreshToken);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(cont.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", cont.Token);

                return new AuthResponse(cont.Token, cont.RefreshToken);
            }
            else
            {
                AuthFailedResponse cont = await response.Content.ReadFromJsonAsync<AuthFailedResponse>();
                return new AuthResponse(cont.Errors);
            }
        }

        public async Task<AuthResponse> Login(UserLoginRequest loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Identity.Login, loginModel);
            if (response.IsSuccessStatusCode)
            {
                AuthSuccessResponse cont = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();
                await _localStorage.SetItemAsync<string>("JWT-Token", cont.Token);
                await _localStorage.SetItemAsync<string>("JWT-RefreshToken", cont.RefreshToken);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(cont.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", cont.Token);

                return new AuthResponse(cont.Token, cont.RefreshToken);
            }
            else
            {
                AuthFailedResponse cont = await response.Content.ReadFromJsonAsync<AuthFailedResponse>();
                return new AuthResponse(cont.Errors);
            }

        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("JWT-Token");
            await _localStorage.RemoveItemAsync("JWT-RefreshToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("JWT-Token");
            var refreshToken = await _localStorage.GetItemAsync<string>("JWT-RefreshToken");
            var tokenDto = JsonSerializer.Serialize(new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");
            var refreshResult = await _httpClient.PostAsync(ApiRoutes.Identity.Refresh, bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            if (!refreshResult.IsSuccessStatusCode)
            {
                await Logout();
                //throw new ApplicationException("Something went wrong during the refresh token action");
                return string.Empty;
            }
            var result = JsonSerializer.Deserialize<AuthSuccessResponse>(refreshContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            await _localStorage.SetItemAsync("JWT-Token", result.Token);
            await _localStorage.SetItemAsync("JWT-RefreshToken", result.RefreshToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            return result.Token;
        }
    }

}
