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
using Microsoft.AspNetCore.Http;

namespace DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Add;
public class AddDangerousFeedCommand : IRequest<string>, IMapFrom<DangerousFeed>
{
    public string Headline { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public DateTime OccuresAtFrom { get; set; }
    public DateTime OccuresAtTill { get; set; }


    public double DepthStartInMeter { get; set; }
    public double DepthEndInMeter { get; set; }
    
    public MultiPolygon? Area { get; set; }

    public ICollection<string> SpeciesIds { get; set; }
}

public class AddDangerousFeedCommandHandler : IRequestHandler<AddDangerousFeedCommand, string>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public AddDangerousFeedCommandHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> Handle(AddDangerousFeedCommand request, CancellationToken cancellationToken)
    {
        var dangerousfeed = _mapper.Map<DangerousFeed>(request);

        await _context.DangerousFeeds.AddAsync(dangerousfeed, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var species = await _context.Species.Where(s => request.SpeciesIds.Contains(s.SpeciesExposedId)).ToListAsync(cancellationToken);
        dangerousfeed.Species = species;
        
        await _context.SaveChangesAsync(cancellationToken);

        return dangerousfeed.DangerousFeedExposedId;
    }
}
