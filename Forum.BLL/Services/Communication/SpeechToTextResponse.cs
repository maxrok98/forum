using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.DAL.Models;

namespace Forum.BLL.Services.Communication
{
    public class SpeechToTextResponse : BaseResponse<SpeechToText>
    {
        public SpeechToTextResponse(SpeechToText sub) : base(sub) { }
        public SpeechToTextResponse(string message) : base(message) { }
    }
}
