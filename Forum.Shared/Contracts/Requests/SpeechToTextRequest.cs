using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Shared.Contracts.Requests
{
    public class SpeechToTextRequest
    {
        public byte[] wavFile { get; set; }
    }
}
