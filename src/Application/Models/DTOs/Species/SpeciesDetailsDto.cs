using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Application.Interfaces;
using DiveSpecies.Application.Mapping;
using DiveSpecies.Domain.Entities;

namespace DiveSpecies.Application.Models.DTOs.SpeciesNS;

public class SpeciesDetailsDto : IMapFrom<Species>
{
    public int SpeciesId { get; set; }
    public string SpeciesExposedId { get; set; } = null!;
    public string Name { get; set; } = string.Empty;

    public int Population { get; set; }

    public SpeciesType SpeciesTypeId { get; set; }
    public WaterType OccuresIn { get; set; }
    public ICollection<SpeciesOccuresAtDto> OccuresAt { get; set; } = new HashSet<SpeciesOccuresAtDto>();

    public ICollection<string> SpeciesImages { get; set; } = new List<string>();

    public void Mapping(Profile p)
    {
        p.CreateMap<Species, SpeciesDetailsDto>()
            .ForMember(d => d.SpeciesId, o => o.MapFrom(src => src.SpeciesExposedId))
            .ForMember(d => d.SpeciesImages, o => o.MapFrom(src => src.SpeciesImages.Select(a => a.Url)))
            ;
    }
}
