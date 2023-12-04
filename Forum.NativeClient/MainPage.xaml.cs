using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.AspNetCore.Components.WebView;

namespace Forum.NativeClient {
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void BlazorWebView_BlazorWebViewInitialized(object sender, BlazorWebViewInitializedEventArgs e)
        {
            //PermissionStatus status = Permissions.CheckStatusAsync<Permissions.Microphone>().Result;

            //if (status != PermissionStatus.Granted)
            //    Permissions.RequestAsync<Permissions.Microphone>();
        }

        private void BlazorWebView_UrlLoading(object sender, UrlLoadingEventArgs e)
        {
            if (e.Url.AbsolutePath.Contains("createpost"))
            {
                PermissionStatus status = Permissions.CheckStatusAsync<Permissions.Microphone>().Result;

                if (status != PermissionStatus.Granted)
                    Permissions.RequestAsync<Permissions.Microphone>();
            }

        }
    }
}