using Forum.Services.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Forum.Services
{
    public class ImageHostService : IImageHostService
    {
        private readonly IHttpClientFactory _clientFactory;
        public ImageHostService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ImageHostResponse> SaveImageAsync(string base64string)
        {
            var httpClient = _clientFactory.CreateClient("image_store");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("image", base64string)
            });

            var result = await httpClient.PostAsync("", content);
            if (!result.IsSuccessStatusCode)
            {
                return null;

            }

            var str = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ImageHostResponse>(str);
        }
    }
}
