using DiveSpecies.Application.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiveSpecies.Rest.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if(ex.GetType() == typeof(ValidationException))
                {
                    await ValidationException(ex as ValidationException, httpContext);
                } else if(ex.GetType() == typeof(ForbiddenAccessException))
                {
                    await ForbiddenAccessException(ex as ForbiddenAccessException, httpContext);
                }
                else if (ex.GetType() == typeof(AlreadyExistsException))
                {
                    await AlreadyExistsException(ex as AlreadyExistsException, httpContext);
                }
                else if(ex.GetType() == typeof(CustomErrorException))
                {
                    await CustomErrorException(ex as CustomErrorException, httpContext);
                } else
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync(ex.Message);
                }
            }

        }

        private async Task ValidationException(ValidationException ex, HttpContext context)
        {
            context.Response.StatusCode = 400;
            StringBuilder sb = new StringBuilder(ex.Message);
            foreach(var error in ex.Errors)
            {
                sb.Append($"\r\n\t{error.Key}:");
                error.Value.ToList().ForEach(suberror =>
                {
                    sb.Append($"\t{suberror}");
                });
            }
            await context.Response.WriteAsync(sb.ToString());

        }

        private async Task ForbiddenAccessException(ForbiddenAccessException ex, HttpContext context)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync(ex.Message);
        }

        private async Task CustomErrorException(CustomErrorException ex, HttpContext context)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(ex));
        }

        private async Task AlreadyExistsException(AlreadyExistsException ex, HttpContext context)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.Message);
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
  
}
