using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Infrastructure.Models;
public class ThirdPartyAPIOptions
{
    public string BaseUrl { get; set; }

    public string? HeaderKey { get; set; }

    public string? Secret { get; set; }
}
