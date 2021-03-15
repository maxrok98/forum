using Forum.Client.Services.Communication;
using Forum.Contracts;
using Forum.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Forum.Client.Services
{
    public class ThreadService : IThreadService
    {
        private readonly HttpClient _httpClient;

        public ThreadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ThreadResponse>> GetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<ThreadResponse>>(ApiRoutes.Threads.GetAll);
        }
        public async Task<ThreadResponse> Get(string id)
        {
            return await _httpClient.GetFromJsonAsync<ThreadResponse>(ApiRoutes.Threads.Get.Replace("{id}", id));
        }

        public async Task<ServiceResponse> DeleteThread(string id)
        {
            var response = await _httpClient.DeleteAsync(ApiRoutes.Threads.Delete.Replace("{id}", id));
            if (response.IsSuccessStatusCode)
            {
                var thread = await response.Content.ReadFromJsonAsync<ThreadResponse>();
                return new ServiceResponse(true, $"You successfully deleted thread {thread.Name}");
            }
            else
            {
                return new ServiceResponse(false, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<ServiceResponse> SubscribeToThread(string ThreadId)
        {
            var response = await _httpClient.PostAsync(ApiRoutes.Threads.Subscribe.Replace("{id}", ThreadId), null);

            if (response.IsSuccessStatusCode)
            {
                return new ServiceResponse(true, await response.Content.ReadAsStringAsync());
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new ServiceResponse(false, "You have to login to subscribe");
            }
            else
            {
                return new ServiceResponse(false, await response.Content.ReadAsStringAsync());
            }
            
        }

    }
}
