using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Webkit;
using Microsoft.Maui;
using File = Java.IO.File;

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

        public override bool OnShowFileChooser(global::Android.Webkit.WebView webView, IValueCallback filePathCallback, FileChooserParams fileChooserParams)
        {
            if (filePathCallback is null)
            {
                return base.OnShowFileChooser(webView, filePathCallback, fileChooserParams);
            }

            //CallFilePickerAsync(filePathCallback, fileChooserParams).GetAwaiter();
            Task.Run(async () =>  await CallFilePickerAsync(filePathCallback, fileChooserParams));
            return true;
        }

        private static async Task CallFilePickerAsync(IValueCallback filePathCallback, FileChooserParams? fileChooserParams)
        {
            var pickOptions = GetPickOptions(fileChooserParams);
            var fileResults = fileChooserParams?.Mode == ChromeFileChooserMode.OpenMultiple ?
                    await FilePicker.PickMultipleAsync(pickOptions) :
                    new[] { await FilePicker.PickAsync(pickOptions) };

            if (fileResults?.All(f => f is null) ?? true)
            {
                // Task was cancelled, return null to original callback
                filePathCallback.OnReceiveValue(null);
                return;
            }

            var fileUris = new List<global::Android.Net.Uri>(fileResults.Count());
            foreach (var fileResult in fileResults)
            {
                if (fileResult is null)
                {
                    continue;
                }

                var javaFile = new File(fileResult.FullPath);
                var androidUri = global::Android.Net.Uri.FromFile(javaFile);

                if (androidUri is not null)
                {
                    fileUris.Add(androidUri);
                }
            }

            filePathCallback.OnReceiveValue(fileUris.ToArray());
            return;
        }

        private static PickOptions? GetPickOptions(FileChooserParams? fileChooserParams)
        {
            var acceptedFileTypes = fileChooserParams?.GetAcceptTypes();
            if (acceptedFileTypes is null ||
                // When the accept attribute isn't provided GetAcceptTypes returns: [ "" ]
                // this must be filtered out.
                (acceptedFileTypes.Length == 1 && string.IsNullOrEmpty(acceptedFileTypes[0])))
            {
                return null;
            }

            var pickOptions = new PickOptions()
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, acceptedFileTypes }
                })
            };
            return pickOptions;
        }
    }
}
