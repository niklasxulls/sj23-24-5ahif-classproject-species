using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiveSpecies.Rest.Swagger;

public class XEnumNamesSchemaFilter : ISchemaFilter
{

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {

        var typeInfo = context.Type;

        if (context.MemberInfo != null || !typeInfo.IsEnum)
            return;
        
        var xEnumVarnamesExtension = new KeyValuePair<string, IOpenApiExtension>(
            "x-enum-varnames", new XEnumVarnamesWriter(typeInfo)
        );

        schema.Extensions.Add(xEnumVarnamesExtension);
        schema.Format = null;

    }
}
