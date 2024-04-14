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
using NetTopologySuite.Geometries;

namespace DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Update;
public class UpdateDangerousFeedCommand : IRequest<string>, IMapFrom<DangerousFeed>
{
    public string DangerousFeedId { get; set; }

    public string Headline { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public DateTime OccuredAtFrom { get; set; }
    public DateTime OccuredAtTill { get; set; }


    public double DepthStartInMeter { get; set; }
    public double DepthEndInMeter { get; set; }
    public MultiPolygon Area { get; set; }  = new MultiPolygon(new Polygon[] { });
        
    public ICollection<string> SpeciesIds { get; set; }

}

public class UpdateDangerousFeedCommandHandler : IRequestHandler<UpdateDangerousFeedCommand, string>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public UpdateDangerousFeedCommandHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateDangerousFeedCommand request, CancellationToken cancellationToken)
    {
        var dangerousfeed = await _context.DangerousFeeds.Where(df => df.DangerousFeedExposedId == request.DangerousFeedId).FirstAsync(cancellationToken);

        dangerousfeed.Headline = request.Headline;
        dangerousfeed.Body = request.Body;

        dangerousfeed.OccuresAtFrom = request.OccuredAtFrom;
        dangerousfeed.OccuresAtTill = request.OccuredAtTill;


        dangerousfeed.DepthStartInMeter = request.DepthStartInMeter;
        dangerousfeed.DepthEndInMeter = request.DepthEndInMeter;
        dangerousfeed.Area = request.Area;

        var species = await _context.Species.Where(s => request.SpeciesIds.Contains(s.SpeciesExposedId)).ToListAsync(cancellationToken);
        dangerousfeed.Species = species;

        await _context.SaveChangesAsync(cancellationToken);

        return dangerousfeed.DangerousFeedExposedId;
    }
}
