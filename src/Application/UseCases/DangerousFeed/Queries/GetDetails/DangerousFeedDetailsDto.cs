using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DiveSpecies.Application.Mapping;
using DiveSpecies.Application.Models.DTOs.SpeciesNS;
using DiveSpecies.Domain.Entities;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Application.UseCases.DangerousFeedNS.Queries.GetDetails;
public class DangerousFeedDetailsDto : IMapFrom<DangerousFeed>
{
    public string DangerousFeedId { get; set; } = null!;

    public string Headline { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public DateTime OccuresAtFrom { get; set; }
    public DateTime OccuresAtTill { get; set; }


    public double DepthStartInMeter { get; set; }
    public double DepthEndInMeter { get; set; }
    public MultiPolygon Area { get; set; }

    public ICollection<SpeciesShallowDto> Species { get; set; }

    public void Mapping(Profile p)
    {
        p.CreateMap<DangerousFeed, DangerousFeedDetailsDto>()
            .ForMember(d => d.DangerousFeedId, o => o.MapFrom(src => src.DangerousFeedExposedId));
    }
}
