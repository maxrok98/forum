using Forum.Components.Services.Communication;
using Forum.Shared.Contracts;
using Forum.Shared.Contracts.Requests;
using Forum.Shared.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Components.Services
{
    public class CognitiveService : ICognitiveService
    {
        private readonly HttpClient _httpClient;

        public CognitiveService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CognitiveResponse> GetSpeechToText(byte[] speech)
        {
            var result = await _httpClient.PostAsJsonAsync(ApiRoutes.Cognitive.SpeechToText, new SpeechToTextRequest { wavFile = speech });
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<SpeechToTextResponse>();
                return new CognitiveResponse(response.Text);
            }
            return new CognitiveResponse(new List<string> { await result.Content.ReadAsStringAsync() });
               
        }
    }
}
