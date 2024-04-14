using System;
using DiveSpecies.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetSpeciesDepthAnalyticsQuery : IRequest<ICollection<GetSpeciesDepthAnalysisGroupDto>>
{

}

public class GetSpeciesDepthAnalyticsQueryHandler : IRequestHandler<GetSpeciesDepthAnalyticsQuery, ICollection<GetSpeciesDepthAnalysisGroupDto>>
{
    private readonly IDiveSpeciesDbContext _context;

    public GetSpeciesDepthAnalyticsQueryHandler(IDiveSpeciesDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<GetSpeciesDepthAnalysisGroupDto>> Handle(GetSpeciesDepthAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var depthData = await _context.Species.SelectMany(a => a.OccuresAt.Select(o => new GetSpeciesDepthAnalysisGroupDto()
        {
            DepthStartInMeter = o.DepthStartInMeter ?? 0,
            DepthEndInMeter = o.DepthEndInMeter ?? 0,
            SpeciesName = o.Species.Name,
            SpeciesId = o.Species.SpeciesExposedId,
        }))
        .ToListAsync(cancellationToken);

        return depthData;
    }
}

