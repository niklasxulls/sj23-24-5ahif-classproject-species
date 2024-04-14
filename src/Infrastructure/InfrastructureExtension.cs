using DiveSpecies.Application.Interfaces;
using DiveSpecies.Application.Interfaces.Services;
using DiveSpecies.Application.Interfaces.Services.Data;
using DiveSpecies.Infrastructure.Models;
using DiveSpecies.Infrastructure.Persistence;
using DiveSpecies.Infrastructure.Services;
using DiveSpecies.Infrastructure.Services.Data;
using DiveSpecies.Infrastructure.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;

namespace DiveSpecies.Infrastructure;

public static class InfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        /*
        *  Configure EF
        */
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<DiveSpeciesDbContext>(options =>
               options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        }
        else
        {
            services.AddDbContext<DiveSpeciesDbContext>(options =>
                options.UseSqlServer(
#if DEBUG
                    configuration.GetConnectionString("DevConnection"),
#else
                    configuration.GetConnectionString("DefaultConnection"),
#endif
                    b => b.UseNetTopologySuite().MigrationsAssembly(typeof(DiveSpeciesDbContext).Assembly.FullName))
                );
        }

        services.AddScoped<IDiveSpeciesDbContext>(provider => provider.GetRequiredService<DiveSpeciesDbContext>());

        /*
        * HTTP Client configurations
        */
        services.AddSingleton<HttpClient>();


        services.AddHttpClient(HttpClientUtil.HTTP_CLIENT_RAPIDAPI_CONFIG, client =>
        {
            ThirdPartyAPIOptions thirdPartyAPIOptions = configuration.GetSection("ThirdParty:RapidAPI").Get<ThirdPartyAPIOptions>();

            client.BaseAddress = new Uri(thirdPartyAPIOptions.BaseUrl);
            client.DefaultRequestHeaders.Add(thirdPartyAPIOptions.HeaderKey!, thirdPartyAPIOptions.Secret);
        });

        services.AddHttpClient(HttpClientUtil.HTTP_CLIENT_MEDIA_CONFIG, client =>
        {
            ThirdPartyAPIOptions thirdPartyAPIOptions = configuration.GetSection("ThirdParty:Media").Get<ThirdPartyAPIOptions>();

            client.BaseAddress = new Uri(thirdPartyAPIOptions.BaseUrl);

            if (!string.IsNullOrEmpty(thirdPartyAPIOptions.HeaderKey))
            {
                client.DefaultRequestHeaders.Add(thirdPartyAPIOptions.HeaderKey!, thirdPartyAPIOptions.Secret);
            }
        });

        /*
        * Data Services
        */
        services.AddTransient<IMediaService, MediaService>();
        services.AddTransient<IFakeDataService, FakeDataService>();
        services.AddTransient<IThirdPartyAPIService, ThirdPartyAPIService>();

        /*
        * Logging 
        */
        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
        });

        services.AddTransient(typeof(ILoggerService<>), typeof(LoggerService<>));

    }

    public static void AddInfrastructureApp(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAuthentication();
        app.UseAuthorization();

    }
}
