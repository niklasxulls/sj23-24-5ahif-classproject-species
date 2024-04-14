using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiveSpecies.Application.Interfaces;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetLatestSearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetLatestSearch;

public class GetSpeciesSearchHistoryQuery : IRequest<ICollection<SpeciesSearchHistoryDto>>
{
    public string UserId { get; set; }
}

public class GetSpeciesSearchHistoryQueryHandler : IRequestHandler<GetSpeciesSearchHistoryQuery, ICollection<SpeciesSearchHistoryDto>>
{
    private readonly IDiveSpeciesDbContext _context;
    private readonly IMapper _mapper;

    public GetSpeciesSearchHistoryQueryHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ICollection<SpeciesSearchHistoryDto>> Handle(GetSpeciesSearchHistoryQuery request, CancellationToken cancellationToken)
    {
        var searchHistory = await _context.SpeciesSearchHistory.Where(sh => sh.UserExposedId == request.UserId)
                                                               .OrderByDescending(sh => sh.SpeciesSearchHistoryId)
                                                               .ProjectTo<SpeciesSearchHistoryDto>(_mapper.ConfigurationProvider)
                                                               .Take(10)
                                                               .AsNoTracking()
                                                               .ToListAsync();

        return searchHistory;
    }
}
