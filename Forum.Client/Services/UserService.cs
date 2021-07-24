using Forum.Shared.Contracts;
using Forum.Shared.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Forum.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PageResponse<UserShortResponse>> GetAll(string page, string pageSize, string searchTerm)
        {
            return await _httpClient.GetFromJsonAsync<PageResponse<UserShortResponse>>(ApiRoutes.User.GetAll + "/?PageNumber=" + page + "&PageSize=" + pageSize + "&userName=" + searchTerm);
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _httpClient.DeleteAsync(ApiRoutes.User.Delete.Replace("{id}", id));
            if (result.IsSuccessStatusCode)
                return true;
            return false;
        }

    }
}
