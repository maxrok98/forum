#if DEBUG
using Azure.AI.Vision.Common;
using Azure.AI.Vision.ImageAnalysis;
#endif
using Forum.BLL.Services.Communication;
using Forum.DAL.Models;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Forum.BLL.Services
{
    public class CognitiveService : ICognitiveService
    {
        private readonly SpeechConfig _speechConfig;
#if DEBUG
        private readonly VisionServiceOptions _visionConfig;
#endif
        private readonly IHttpClientFactory _httpClientFactory;
        public CognitiveService(SpeechConfig speechConfig,
#if DEBUG
            VisionServiceOptions visionServiceOptions,
#endif
            IHttpClientFactory httpClientFactory)
        {
            _speechConfig = speechConfig;
#if DEBUG
            _visionConfig = visionServiceOptions;
#endif
            _httpClientFactory = httpClientFactory;
        }
        public async Task<SpeechToTextResponse> SpeechToText(byte[] wavFile)
        {
            //File.WriteAllBytes("C:/users/mroko/downloads/TestSampleFromBytes.wav", wavFile);
            var waveFormat = AudioStreamFormat.GetWaveFormatPCM(8000, 16, 1);
            try
            {
                wavFile = await DenoiseSound(wavFile);
            }
            catch
            {
                waveFormat = AudioStreamFormat.GetWaveFormatPCM(44100, 16, 2);
            }


            var reader = new BinaryReader(new MemoryStream(wavFile));
            using var audioConfigStream = AudioInputStream.CreatePushStream(waveFormat);
            using var audioConfig = AudioConfig.FromStreamInput(audioConfigStream);
            using var speechRecognizer = new SpeechRecognizer(_speechConfig, audioConfig);

            //audioConfigStream.Write(wavFile, wavFile.Length);
            byte[] readBytes;
            do
            {
                readBytes = reader.ReadBytes(1024);
                audioConfigStream.Write(readBytes, readBytes.Length);
            } while (readBytes.Length > 0);

            var result = await speechRecognizer.RecognizeOnceAsync();
            Console.WriteLine($"RECOGNIZED: Text={result.Text}");

            switch (result.Reason)
            {
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    return new SpeechToTextResponse("Speech could not be recognized.");
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(result);
                    return new SpeechToTextResponse($"CANCELED: Reason={cancellation.Reason}");
            }
            return new SpeechToTextResponse(new SpeechToText { Text = result.Text });
        }

#if DEBUG
        public async Task<string> TagsFromImage(string imageUrl)
        {
            using var imageSource = VisionSource.FromUrl(
            new Uri(imageUrl));

            var analysisOptions = new ImageAnalysisOptions()
            {
                Features = ImageAnalysisFeature.Tags,
                Language = "en",
                GenderNeutralCaption = true
            };

            using var analyzer = new ImageAnalyzer(_visionConfig, imageSource, analysisOptions);
            var result = await analyzer.AnalyzeAsync();

            StringBuilder tags = new("");
            if(result.Reason == ImageAnalysisResultReason.Analyzed)
            {
                if(result.Tags != null)
                {
                    foreach(var tag in result.Tags)
                    {
                        if (tag.Confidence > 0.9)
                        {
                            tags.Append(" " + tag.Name.Replace(" ", "_"));
                        }
                    }
                }
            }
            return tags.ToString();
        }
#endif

        private async Task<byte[]> DenoiseSound(byte[] wavFile)
        {
            var httpClient = _httpClientFactory.CreateClient("denoiser");
            var payload = new
            {
                wav_bytes = Convert.ToBase64String(wavFile)
            };
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync("", content);

            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadFromJsonAsync<DenoisedResponce>();
            return Convert.FromBase64String(responseData.wav_bytes);
        }
    }

    internal class DenoisedResponce
    {
        public string wav_bytes { get; set; }
    }
}
