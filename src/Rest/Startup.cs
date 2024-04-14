using DiveSpecies.Application.Interfaces.Services;
using DiveSpecies.Infrastructure;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using DiveSpecies.Application;
using DiveSpecies.Rest.Middleware;
using System.Reflection;
using DiveSpecies.Application.Interfaces;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiveSpecies.Rest.Swagger;

namespace DiveSpecies.Rest;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure(Configuration);
        services.AddApplication();

        services.AddCors();
        services.AddControllers().AddJsonOptions(options =>
        {

            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddHttpContextAccessor();

        services.AddSignalR().AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            options.PayloadSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        }); ;

        ////Eager Loading
        services.AddMvc();


        services.AddSwagger();
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //configure method is the process of adding middleware components to the app pipeline
    //because middleware components are sequal, so they work one after each other the order of adding the middle
    //ware components in the configure method is very important. 
    //The task of a middleware component is to handle HTTP requests and responses, after the handle they can invoke
    //the next middleware component in the "row"
    //https://dotnettutorials.net/lesson/asp-net-core-request-processing-pipeline/#:~:text=The%20ASP.NET%20Core%20request%20processing%20pipeline%20consists%20of%20a,component%20using%20the%20next%20method.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //checks if host is developed
        if (env.IsDevelopment())
        {
            //when app is running in development enviroment catches exceptions that are 
            //being caused in the middleware pipeline and informates about them detailed.
            //https://dotnettutorials.net/lesson/developer-exception-page-middleware-asp-net-core/
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0
            //adds swagger API and UI to application
        }
        //just for dev
        app.UseDeveloperExceptionPage();


        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DiveSpeciesWebAPI v1"));

        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {

                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                await Task.CompletedTask;
            });
        });

        app.UseHttpsRedirection();
        //app.UseRouting();

        //just for dev 
        app.UseRouting();

        app.UseCors(options =>
        //options./*WithOrigins("http://localhost:4200")*/
        options
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());
        //Exposes etxtra Header Retry-After when needed. Usually Javascript does only have access on specific headers
        //without approving the header in the ExposeHeaders Method, it won't be able to see the header in any response 
        //header method like from axios.
        //Furthermore, WithExposedHeaders accepts params, which means, that infinite (until out of memory :)) arguments are
        //possible.
        //.WithExposedHeaders("Retry-After"));
        app.AddInfrastructureApp(env);

        app.UseMiddleware<LoggerMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
