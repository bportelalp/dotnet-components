using BP.Components.Blazor.UI.Notification;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

namespace BP.Samples.BlazorWebAssembly.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<ToastService>();
            builder.Services.AddLocalization(r => r.ResourcesPath = "Resources");
            await builder.Build().RunAsync();
        }
    }
}