using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Components.Services.Communication
{
    public class CognitiveResponse
    {
        public bool Success { get; set; } = false;
        public IEnumerable<string> Errors { get; set; }
        public string Text { get; set; }

        public CognitiveResponse(string text)
        {
            Success = true;
            Text = text;
        }
        public CognitiveResponse(IEnumerable<string> errors)
        {
            Success = false;
            Errors = errors;
        }
    }
}
