using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Domain.Entities;
public class SightingImage
{
    public int SightingImageId { get; set; }

    public string Url { get; set; }

    public int SightingId { get; set; }

    public Sighting Sighting { get; set; }

}
