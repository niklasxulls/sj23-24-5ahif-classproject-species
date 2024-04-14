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
public class SpeciesImageDto : IMapFrom<SpeciesImage>
{
    public string? Url { get; set; }

    public void Mapping(Profile p)
    {
        p.CreateMap<SpeciesImage, SpeciesImageDto>()
            .ReverseMap();
    }
}
