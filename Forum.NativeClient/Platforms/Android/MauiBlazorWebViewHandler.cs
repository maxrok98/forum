using Microsoft.AspNetCore.Components.WebView.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.NativeClient.Platforms.Android
{
    class MauiBlazorWebViewHandler : BlazorWebViewHandler
    {
        protected override global::Android.Webkit.WebView CreatePlatformView()
        {
            var view = base.CreatePlatformView();

            view.SetWebChromeClient(new MyWebChromeClient());

            return view;
        }
    }
}
