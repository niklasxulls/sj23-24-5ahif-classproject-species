using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DiveSpecies.Infrastructure.Persistence.Configurations.Converter;
public class JsonValueConverter<T> : ValueConverter<T, string>
{
    public JsonValueConverter(
        Expression<Func<T, string>> convertToProviderExpression, 
        Expression<Func<string, T>> convertFromProviderExpression, 
        ConverterMappingHints? mappingHints = null
      ) 
        : 
     base(
        value => JsonSerializer.Serialize(value, new JsonSerializerOptions()),
        json => JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()),
        mappingHints
      )
    {
    }
}
