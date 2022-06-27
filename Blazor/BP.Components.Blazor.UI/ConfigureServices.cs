using BP.Components.Blazor.UI.FrontendUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Components.Blazor.UI
{
    public static class ConfigureServices
    {
        public static void ConfigureBlazorUIServices(this IServiceCollection services)
        {
            services.AddScoped<DomAccessService>();
            services.AddScoped<LocalStorageService>();
            services.AddScoped<NotificationService>();
        }
    }
}
