using System;
using MediatR;
using System.Collections.Generic;
using DiveSpecies.Application.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class DateAnalyticsQuery : IRequest<List<DateAnalyticsDto>>
{
    
}

public class DateAnalyticsQueryHandler : IRequestHandler<DateAnalyticsQuery, List<DateAnalyticsDto>>
{
    private readonly IDiveSpeciesDbContext _context;
    private readonly IMapper _mapper;

    public DateAnalyticsQueryHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DateAnalyticsDto>> Handle(DateAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var dateAnalytics = await _context.Sightings.GroupBy(s => new { s.CreatedAt.Date })
                                                    .Select(g => new DateAnalyticsDto { Date = g.Key.Date, Count = g.Count() })
                                                    .ToListAsync(cancellationToken);

        return dateAnalytics;
    }
}
