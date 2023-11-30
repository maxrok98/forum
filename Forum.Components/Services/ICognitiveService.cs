using Forum.Components.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Components.Services
{
    public interface ICognitiveService
    {
        Task<CognitiveResponse> GetSpeechToText(byte[] speech);
    }
}
