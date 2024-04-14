using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Application.Mapping;
using DiveSpecies.Domain.Entities;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
public class AddSighting : IMapFrom<Sighting>
{
    public string SpeciesExposedId { get; set; }
    public double? DepthStartInMeter { get; set; }
    public double? DepthEndInMeter { get; set; }
    public double DiveIntoInMinutes { get; set; }
    public MultiPolygon Area { get; set; }
    public int Count { get; set; }
    public IFormFileCollection Images { get; set; }
}
