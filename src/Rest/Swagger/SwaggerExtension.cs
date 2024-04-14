using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiveSpecies.Rest.Swagger;

public static class SwaggerExtension
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiveSpecies Rest Api", Version = "v1" });



            c.CustomOperationIds((desc) =>
            {
                return ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)desc.ActionDescriptor).ActionName;
            });

            c.SchemaGeneratorOptions.UseInlineDefinitionsForEnums = false;

            c.AddSecurity();
            c.AddLingualEnumDefinitions();
            c.AddQueriesToSwagger();
            c.AddDtosToSwagger();
            c.AddSwaggerDocumentation();
            c.AddOtherFilter();
            c.DescribeAllParametersInCamelCase();

        });
    }

    private static void AddLingualEnumDefinitions(this SwaggerGenOptions c)
    {
        c.SchemaFilter<XEnumNamesSchemaFilter>();
        c.SchemaFilter<ValueObjectsSchemaFilter>();
    }

    private static void AddOtherFilter(this SwaggerGenOptions c)
    {
        //c.ParameterFilter<DictionarySchemaFilter>();
    }

    private static void AddSwaggerDocumentation(this SwaggerGenOptions c)
    {
        var xmlFilenames = AppDomain.CurrentDomain.GetAssemblies()
                                                  .Where(t => t.FullName.StartsWith("DiveSpecies"))
                                                  .Select(x => $"{x.GetName().Name}.xml");

        foreach(var xmlFilename in xmlFilenames)
        {
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        }
    }

 
    public static void AddDtosToSwagger(this SwaggerGenOptions c)
    {

    }

    public static void AddQueriesToSwagger(this SwaggerGenOptions c)
    {
        //c.DocumentFilter<CustomModelDocumentFilter<TestQuery>>();
    }

    public static void AddSecurity(this SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("diveSpeciesSecurity", new OpenApiSecurityScheme()
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"

        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "diveSpeciesSecurity",
                    }
                },
                new string[]{}
            }
        });
    }

}
