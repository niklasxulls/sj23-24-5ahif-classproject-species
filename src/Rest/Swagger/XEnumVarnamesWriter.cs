using Microsoft.OpenApi;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Writers;

namespace DiveSpecies.Rest.Swagger;

public class XEnumVarnamesWriter : IOpenApiExtension
{
    private readonly Type _type;
    private readonly ICollection<string> _propnames;

    public XEnumVarnamesWriter(Type type)
    {
        _type = type;
    }

    public XEnumVarnamesWriter(ICollection<string> propnames)
    {
        _propnames = propnames;
    }

    public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
    {
        List<string> propnames = new();
        
        if(_propnames == null)
        {
            var enumValues = _type.GetEnumValues();

            foreach(var enumValue in enumValues)
            {
                var propname = enumValue.ToString();
                propnames.Add(propname);
            }
        } else
        {
            propnames = _propnames.ToList();
        }

        var str = $"[{string.Join(",\n", propnames.Select(x => $"\"{x}\""))}]" ;

        writer.WriteRaw(str);
        var content = writer.ToString();
    }
}
