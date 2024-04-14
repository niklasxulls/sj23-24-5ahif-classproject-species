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
using AutoMapper.QueryableExtensions;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries;
using DiveSpecies.Application.Models;
using NetTopologySuite.Geometries;
using DiveSpecies.Application.Models.DTOs.SpeciesNS;

namespace DiveSpecies.Application.UseCases.DangerousFeedNS.Queries.GetByDate;
public class SearchDangerousFeedQuery : IRequest<PagedResult<DangerousFeedShallowDto>>
{
    public string? SearchTerm { get; set; }
    
    public DateTime OccuresAtFrom { get; set; }
    public DateTime OccuresAtTill { get; set; }

    public double? DepthStartInMeter { get; set; }
    public double? DepthEndInMeter { get; set; }
    
    public MultiPolygon? Area { get; set; }
    public ICollection<string>? SpeciesIds { get; set; }

    public Paging Paging { get; set; }
}

public class SearchDangerousFeedQueryHandler : IRequestHandler<SearchDangerousFeedQuery, PagedResult<DangerousFeedShallowDto>>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public SearchDangerousFeedQueryHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<DangerousFeedShallowDto>> Handle(SearchDangerousFeedQuery request, CancellationToken cancellationToken)
    {

        var query = _context.DangerousFeeds.Where(a => 
            (a.OccuresAtFrom >= request.OccuresAtFrom && a.OccuresAtFrom <= request.OccuresAtTill)
            || (a.OccuresAtTill >= request.OccuresAtFrom && a.OccuresAtTill <= request.OccuresAtTill)
            || (a.OccuresAtFrom <= request.OccuresAtFrom && a.OccuresAtTill >= request.OccuresAtTill)
        );
        
        
        // build query
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var loweredSearchTerm = request.SearchTerm.ToLower();
            query = query.Where(s => s.Headline.ToLower().Contains(loweredSearchTerm) || s.Body.ToLower().Contains(loweredSearchTerm));
        }


        if (request.DepthStartInMeter.GetValueOrDefault() > 0 || request.DepthEndInMeter.GetValueOrDefault() > 0)
        {
            var start = request.DepthStartInMeter.GetValueOrDefault() > 0 ? request.DepthStartInMeter.GetValueOrDefault() : 0;
            var end = request.DepthEndInMeter.GetValueOrDefault() > 0 ? request.DepthEndInMeter.GetValueOrDefault() : double.MaxValue;

            query = query.Where(aa => (aa.DepthStartInMeter >= start && aa.DepthStartInMeter <= end)
                                      || (aa.DepthEndInMeter >= start && aa.DepthEndInMeter <= request.DepthEndInMeter)
                               );
        }

        if (request.SpeciesIds?.Any() ?? false)
        {
            query = query.Where(r => r.Species.Any(s => request.SpeciesIds.Contains(s.SpeciesExposedId)));
        }

        if (request.Area != null)
        {
            query = query.Where(aa => request.Area.Contains(aa.Area));
        }


        var dangerousFeeds = await query.Skip(request.Paging.Offset)
                                        .Take(request.Paging.Size)
                                        .AsNoTracking()
                                        .OrderByDescending(df => df.OccuresAtFrom)
                                        .ProjectTo<DangerousFeedShallowDto>(_mapper.ConfigurationProvider)
                                        .ToListAsync(cancellationToken);


        return new PagedResult<DangerousFeedShallowDto>()
        {
            Items = dangerousFeeds,
            Paging = request.Paging,
        };
    }

    
}
