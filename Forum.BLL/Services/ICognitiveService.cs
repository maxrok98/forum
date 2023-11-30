using Forum.BLL.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.BLL.Services
{
    public interface ICognitiveService
    {
        Task<SpeechToTextResponse> SpeechToText(byte[] wavFile);
    }
}
