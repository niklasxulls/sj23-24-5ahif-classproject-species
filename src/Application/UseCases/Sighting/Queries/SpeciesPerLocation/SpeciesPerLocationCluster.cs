using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Application.Mapping;
using DiveSpecies.Application.Models.DTOs.SpeciesNS;
using DiveSpecies.Domain.Entities;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Application.UseCases.SightingNS.Queries.SpeciesPerLocation;
public class SpeciesPerLocationCluster
{
    public Point Centroid { get; set; }
    public ICollection<SpeciesPerLocationDto> Species { get; set; } = new List<SpeciesPerLocationDto>();

}

public class SpeciesPerLocationDto : IMapFrom<Sighting>
{
    public SpeciesShallowDto Species { get; set; }
    public MultiPolygon Area { get; set; } = new MultiPolygon(new Polygon[] { });
    public int Count { get; set; }
}
