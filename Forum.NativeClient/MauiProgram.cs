using Blazored.LocalStorage;
using Forum.Components;
using Forum.Components.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Sotsera.Blazor.Toaster.Core.Models;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Routing;

namespace Forum.NativeClient {
  public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
      var builder = MauiApp.CreateBuilder();
      builder
        .UseMauiApp<App>()
        .ConfigureFonts(fonts => {
          fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        });

      builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddScoped(sp => new HttpClient 
        {
#if ANDROID
            BaseAddress = new Uri("http://10.0.2.2:5000") 
#else
            BaseAddress = new Uri("http://localhost:5000")
#endif
        }
        .EnableIntercept(sp));
        builder.Services.AddHttpClientInterceptor();
        builder.Services.AddScoped<HttpInterceptorService>();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
        builder.Services.AddScoped<RefreshTokenService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<IThreadService, ThreadService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddToaster(config =>
        {
            //example customizations
            config.PositionClass = Defaults.Classes.Position.BottomRight;
            config.PreventDuplicates = true;
            config.NewestOnTop = false;
        });

      return builder.Build();
	}
  }
}