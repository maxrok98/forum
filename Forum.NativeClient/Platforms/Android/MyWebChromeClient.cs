using Android.Webkit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.NativeClient.Platforms.Android
{
    internal class MyWebChromeClient : WebChromeClient
    {
        //public MyWebChromeClient(IWebViewHandler handler) : base((WebViewHandler)handler)
        //{

        //}

        public override void OnPermissionRequest(PermissionRequest request)
        {
            // Process each request
            foreach (var resource in request.GetResources())
            {
                // Check if the web page is requesting permission to the microphone
                if (resource.Equals(PermissionRequest.ResourceAudioCapture, StringComparison.OrdinalIgnoreCase))
                {
                    // Get the status of the .NET MAUI app's access to the microphone
                    PermissionStatus status = Permissions.CheckStatusAsync<Permissions.Microphone>().Result;

                    // Deny the web page's request if the app's access to the microphone is not "Granted"
                    if (status != PermissionStatus.Granted)
                        request.Deny();
                    else
                        request.Grant(request.GetResources());

                    return;
                }
            }

            base.OnPermissionRequest(request);
        }
    }
}
