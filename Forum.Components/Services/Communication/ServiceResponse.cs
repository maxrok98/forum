using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Components.Services.Communication
{
    public class ServiceResponse
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        public ServiceResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
