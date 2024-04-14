using System.Collections.Generic;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiveSpecies.Rest.Swagger;

public class DictionarySchemaFilter : IParameterFilter
{

    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (!parameter.In.HasValue || parameter.In.Value != ParameterLocation.Query)
            return;

        if (parameter.Schema?.Type == "array" || parameter.Schema?.Type == "object")
        {
            var value = null as IOpenApiExtension;
            parameter.Extensions.TryGetValue("explode", out value);

            var content = new OpenApiMediaType();
            content.Schema = parameter.Schema;
            parameter.Content.Add("application/json", content);

            if (value == null)
                parameter.Extensions.Add("explode", new OpenApiBoolean(true));
         

            parameter.Extensions.TryGetValue("style", out value);
            if (value == null)
                parameter.Extensions.Add("style", new OpenApiString("deepObject"));


        }
        parameter.Schema = null;
    }
}
