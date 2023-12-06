using Azure.AI.Vision.Common;
//using Azure.AI.Vision.ImageAnalysis;
using Forum.BLL.Services.Communication;
using Forum.DAL.Models;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Forum.BLL.Services
{
    public class CognitiveService : ICognitiveService
    {
        private readonly SpeechConfig _speechConfig;
        //private readonly VisionServiceOptions _visionConfig;
        public CognitiveService(SpeechConfig speechConfig)//, VisionServiceOptions visionServiceOptions)
        {
            _speechConfig = speechConfig;
            //_visionConfig = visionServiceOptions;
        }
        public async Task<SpeechToTextResponse> SpeechToText(byte[] wavFile)
        {
            //File.WriteAllBytes("C:/users/mroko/downloads/TestSampleFromBytes.wav", wavFile);
            var reader = new BinaryReader(new MemoryStream(wavFile));
            using var audioConfigStream = AudioInputStream.CreatePushStream(AudioStreamFormat.GetWaveFormatPCM(44100, 16, 2));
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

        public async Task<string> TagsFromImage(string imageUrl)
        {
            //using var imageSource = VisionSource.FromUrl(
            //new Uri(imageUrl));

            //var analysisOptions = new ImageAnalysisOptions()
            //{
            //    Features = ImageAnalysisFeature.Tags,
            //    Language = "en",
            //    GenderNeutralCaption = true
            //};

            //using var analyzer = new ImageAnalyzer(_visionConfig, imageSource, analysisOptions);
            //var result = await analyzer.AnalyzeAsync();

            //StringBuilder tags = new("");
            //if(result.Reason == ImageAnalysisResultReason.Analyzed)
            //{
            //    if(result.Tags != null)
            //    {
            //        foreach(var tag in result.Tags)
            //        {
            //            if (tag.Confidence > 0.9)
            //            {
            //                tags.Append(" " + tag.Name.Replace(" ", "_"));
            //            }
            //        }
            //    }
            //}
            //return tags.ToString();
            return "";
        }
    }
}
