using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiveSpecies.Infrastructure.Models;
public class RapidApiFishSchema
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("img_src_set")]
    public JsonElement ImgSrcSet { get; set; }

    public string ImgSrcSetTwoX
    {
        get
        {
            if (ImgSrcSet.ValueKind == JsonValueKind.Object)
            {
                // Look for "2x" case-insensitively
                foreach (var property in ImgSrcSet.EnumerateObject())
                {
                    if (string.Equals(property.Name, "2x", StringComparison.OrdinalIgnoreCase))
                    {
                        return property.Value.GetString();
                    }
                }
            }
            return null;
        }
    }
}
