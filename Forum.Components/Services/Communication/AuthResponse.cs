using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Client.Services.Communication
{
    public class AuthResponse
    {
        public bool Success { get; set; } = false;
        public IEnumerable<string> Errors { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public AuthResponse(string token, string refreshToken)
        {
            Success = true;
            Token = token;
            RefreshToken = refreshToken;
        }
        public AuthResponse(IEnumerable<string> errors)
        {
            Success = false;
            Errors = errors;
        }
    }
}
