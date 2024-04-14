using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiveSpecies.Domain.Entities.Defaults;
public class Json<T>
{
    public T? Data { get; set; }
    public string Serialized
    {
        get { return JsonConvert.SerializeObject(Data); }
        set
        {
            if (string.IsNullOrEmpty(value)) return;

            var deserialized = JsonConvert.DeserializeObject<T>(value);
            Data = deserialized ?? default(T);

        }
    }
}
