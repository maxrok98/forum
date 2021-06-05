using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using Sotsera.Blazor.Toaster.Core.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Forum.Client.Services;
using System.Reflection;
using System.Linq;

namespace Forum.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            foreach(var s in Assembly.GetExecutingAssembly().GetTypes().Select(x => x.Namespace).ToArray())
            {
                Console.WriteLine(s);
            }
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddHttpClient("BlazorApp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
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


            await builder.Build().RunAsync();
        }
    }
}
