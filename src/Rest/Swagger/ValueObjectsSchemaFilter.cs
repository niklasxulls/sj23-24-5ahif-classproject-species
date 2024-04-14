using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiveSpecies.Rest.Swagger;

public class ValueObjectsSchemaFilter : ISchemaFilter
{

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var typeInfo = context.Type;

        if (!typeInfo.IsSubclassOf(typeof(ValueObject)))
            return;

        // get members which represent options of the value objects, therefore are of the type of the value object
        var members = typeInfo.GetFields(BindingFlags.Static | BindingFlags.Public)
                              .Where(m => m.FieldType == typeInfo);
        // get value of static members
        var propnames = members.Select(m => m.GetValue(null).ToString()).ToList();



        // apply string enum values
        schema.Enum = new List<IOpenApiAny>() { };

        foreach(var propname in propnames)
        {
            schema.Enum.Add(new OpenApiString(propname));
        }

        // since all value objects are identified by a string the type can be set to string
        schema.Type = "string";
        schema.Properties = null;
        schema.AdditionalProperties = null;
        schema.AdditionalPropertiesAllowed = true;
        schema.Format = null;

    }
}
