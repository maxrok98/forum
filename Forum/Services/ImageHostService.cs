using Forum.Services.Communication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

            //var jsonString = JsonConvert.SerializeObject(new KeyValuePair<string, string>("image", base64string));
            var content = new StringContent(base64string, Encoding.UTF8, "application/x-www-form-urlencoded");

            var form = new MultipartFormDataContent();
            form.Add(content, "image");


            var result = await httpClient.PostAsync("", form);
            if (!result.IsSuccessStatusCode)
            {
                return null;

            }

            var str = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ImageHostResponse>(str);
        }
    }
}
