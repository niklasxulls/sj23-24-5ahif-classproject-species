using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DiveSpecies.Application.Mapping;
using DiveSpecies.Domain.Entities;

namespace DiveSpecies.Application.Models.DTOs.SpeciesNS;
public class SpeciesShallowDto : IMapFrom<Species>
{
    public string SpeciesId { get; set; }
    public string Name { get; set; } = string.Empty;

    public int Population { get; set; }
    public SpeciesType SpeciesTypeId { get; set; }

    public WaterType OccuresIn { get; set; }


    public ICollection<SpeciesOccuresAtDto> OccuresAt { get; set; } = new List<SpeciesOccuresAtDto>();

    public ICollection<string> SpeciesImages { get; set; } = new List<string>();


    public void Mapping(Profile p)
    {
        p.CreateMap<Species, SpeciesShallowDto>()
            .ForMember(d => d.SpeciesId, o => o.MapFrom(src => src.SpeciesExposedId))
            .ForMember(d => d.SpeciesImages, o => o.MapFrom(src => src.SpeciesImages.Select(a => a.Url)))
            ;
    }
}
