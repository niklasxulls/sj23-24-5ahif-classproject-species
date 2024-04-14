using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Domain.Entities;

/// <summary>
/// A species can occur at multiple places at different water levels, which is represented by this entity
/// </summary>
public class SpeciesOccuresAt
{
    public int SpeciesOccuresAtId { get; set; }
    
    public int SpeciesId { get; set; }
    public Species Species { get; set; } = null!;

    public Polygon? Area { get; set; } = Polygon.Empty;
    public double? DepthStartInMeter { get; set; }
    public double? DepthEndInMeter { get; set; }
}
