using DiveSpecies.Application.Interfaces;
using DiveSpecies.Application.Interfaces.Services;
using DiveSpecies.Application.Interfaces.Services.Data;
using DiveSpecies.Infrastructure.Persistence;
using DiveSpecies.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiveSpecies.Rest;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                Console.WriteLine("Before gathering services");

                var context = services.GetRequiredService<DiveSpeciesDbContext>();
                var thirdPartyAPIService = services.GetRequiredService<IThirdPartyAPIService>();
                var fakeDataService = services.GetRequiredService<IFakeDataService>();

                Console.WriteLine("Before creating db");
                //context.Database.EnsureDeleted(); // enable only for filling testing purposes
                try
                {
                    context.Database.EnsureCreated();
                } catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                Console.WriteLine("After creating db");

                try
                {
                    await DiveSpeciesDbContextSeed.SeedSampleData(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Seed Sample data error: " + ex.Message);
                }

                try
                {
                    await DiveSpeciesDbContextSeed.TryAddSpeciesFromThirdPartyToDB(context, thirdPartyAPIService, fakeDataService);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Third party species error: " + ex.Message);
                    Console.WriteLine("Third party species stacktrace: " + ex.StackTrace);
                }



                Console.WriteLine("After seeded sample data");
            }
            finally
            {
                await host.RunAsync();
            }
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                // dev
                webBuilder.CaptureStartupErrors(true).UseSetting("detailedErrors", "true");
                webBuilder.UseStartup<Startup>();
            });
}
