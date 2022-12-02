using BP.Samples.EFTests.Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BP.Samples.EFTests.IoC
{
    public static class Startup
    {
        public static IServiceCollection ConfigureIoC(this IServiceCollection services, IConfiguration configuration)
        {
            string cnx = configuration.GetConnectionString("DB");
            services.AddDbContext<EFTestContext>(builder => builder.UseNpgsql(cnx));
            return services;
        }
    }
}