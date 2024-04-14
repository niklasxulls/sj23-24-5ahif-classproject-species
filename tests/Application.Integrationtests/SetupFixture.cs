using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiveSpecies.Rest;
using Moq;
using System.Reflection;
using Xunit;
using MediatR;
using DiveSpecies.Infrastructure.Persistence;

namespace Application.IntegrationTests;

public class SetupFixture : IDisposable
{
    private readonly IConfigurationRoot _configuration;
    public IServiceScopeFactory _scopeFactory;

    public SetupFixture()
    {
        //var builder = new ConfigurationBuilder().a
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json", true, true)
            .AddEnvironmentVariables();


        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();




        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == Assembly.GetAssembly(typeof(Startup))!.FullName));

        startup.ConfigureServices(services);


        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();


        CreateDB();
    }

    private void CreateDB()
    {
        //only asp net core creates scopes automatically (e.g for every webrequest)
        //-> we have to create scopes our own
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DiveSpeciesDbContext>();

        context.Database.EnsureCreated();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }


    public void Dispose()
    {
        
    }
}

[CollectionDefinition("global")]
public class SetupFixtureCollectionDefinition : ICollectionFixture<SetupFixture>
{

}
