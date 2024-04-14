using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DiveSpecies.Application.Mapping;
using DiveSpecies.Domain.Entities;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Application.Models.DTOs.SpeciesNS;

public class SpeciesOccuresAtDto : IMapFrom<SpeciesOccuresAt>
{
    public Polygon Area { get; set; } = Polygon.Empty;
    public double? DepthStartInMeter { get; set; }
    public double? DepthEndInMeter { get; set; }

    public void Mapping(Profile p)
    {
        p.CreateMap<SpeciesOccuresAt, SpeciesOccuresAtDto>()
            .ReverseMap();
    }
}
