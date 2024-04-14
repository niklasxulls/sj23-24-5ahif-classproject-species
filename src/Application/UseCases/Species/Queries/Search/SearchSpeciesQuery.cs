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
using DiveSpecies.Application.Extensions;

namespace DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetDetails;

public class SearchSpeciesQuery : IRequest<PagedResult<SpeciesShallowDto>>, IMapFrom<SpeciesSearchHistory>
{
    public string UserId { get; set; }

    public string SearchTerm { get; set; } = string.Empty;
    public SpeciesType? SpeciesTypeId { get; set; }

    public WaterType? OccuresIn { get; set; }

    public MultiPolygon? Area { get; set; }
    public int? Population { get; set; }
    public NumberParameterOperator? PopulationOperator { get; set; }
    public double? DepthStartInMeter { get; set; }
    public double? DepthEndInMeter { get; set; }


    public SpeciesSearchSortBy SortBy { get; set; }
    public bool SortDesc { get; set; }
    public Paging Paging { get; set; }
}

public class SearchSpeciesQueryHandler : IRequestHandler<SearchSpeciesQuery, PagedResult<SpeciesShallowDto>>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public SearchSpeciesQueryHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<SpeciesShallowDto>> Handle(SearchSpeciesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Species.AsQueryable();

        // build query
        if(!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(s => s.Name.ToLower().Contains(request.SearchTerm.ToLower()));
        }

        if(request.OccuresIn != null)
        {
            query = query.Where(a => a.OccuresIn == request.OccuresIn);
        }

        if(request.Area != null)
        {
            query = query.Where(a => a.OccuresAt.Any(aa => request.Area.Contains(aa.Area)));
        }

        /*        if(request.DepthStartInMeter.GetValueOrDefault() > 0 || request.DepthEndInMeter.GetValueOrDefault() > 0)
                {
                    var start = request.DepthStartInMeter.GetValueOrDefault() > 0 ? request.DepthStartInMeter.GetValueOrDefault() : 0;
                    var end = request.DepthEndInMeter.GetValueOrDefault() > 0 ? request.DepthEndInMeter.GetValueOrDefault() : double.MaxValue;

                    query = query.Where(a => a.OccuresAt.Any(aa => (aa.DepthStartInMeter >= start && aa.DepthStartInMeter <= end) 
                                                                   || (aa.DepthEndInMeter >= start && aa.DepthEndInMeter <= request.DepthEndInMeter)
                                                            ));
                }*/

        if (request.DepthStartInMeter.GetValueOrDefault() > 0 || request.DepthEndInMeter.GetValueOrDefault() > 0)
        {
            var start = request.DepthStartInMeter.GetValueOrDefault() > 0 ? request.DepthStartInMeter.GetValueOrDefault() : 0;
            var end = request.DepthEndInMeter.GetValueOrDefault() > 0 ? request.DepthEndInMeter.GetValueOrDefault() : double.MaxValue;

            query = query.Where(a => a.OccuresAt.Any(aa => aa.DepthEndInMeter <= end && aa.DepthStartInMeter >= start));
        }

        if (request.SpeciesTypeId != null)
        {
            query = query.Where(r => r.SpeciesTypeId == request.SpeciesTypeId);
        }

        query = query.FilterNumber(a => a.Population, request.Population, request.PopulationOperator);



        IOrderedQueryable<Species> sortedQuery = null!;

        switch(request.SortBy)
        {
            case SpeciesSearchSortBy.Latest:
                sortedQuery = query.SortBy(a => _context.Sightings.Any(a => a.SpeciesId == a.SpeciesId && a.Dive.UserExposedId == request.UserId), desc: request.SortDesc);
                break;
            case SpeciesSearchSortBy.Population:
                sortedQuery = query.SortBy(a => a.Population, desc: request.SortDesc);
                break;
            case SpeciesSearchSortBy.Name:
                sortedQuery = query.SortBy(a => a.Name, desc: request.SortDesc);
                break;
        }


        // query
        var species = await query.Skip(request.Paging.Offset)
                                 .Take(request.Paging.Size)
                                 .AsNoTracking()
                                 .ProjectTo<SpeciesShallowDto>(_mapper.ConfigurationProvider)
                                 .ToListAsync(cancellationToken);

        // Create and save search history
        var searchHistory = _mapper.Map<SpeciesSearchHistory>(request);

        await _context.SpeciesSearchHistory.AddAsync(searchHistory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new PagedResult<SpeciesShallowDto>()
        {
            Items = species,
            Paging = request.Paging,
        };
    }
}
