using Forum.BLL.Services.Communication;
using Forum.DAL.Models;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.BLL.Services
{
    public class CognitiveService : ICognitiveService
    {
        private readonly SpeechConfig _speechConfig;
        public CognitiveService(SpeechConfig speechConfig)
        {
            _speechConfig = speechConfig;
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
    }
}
