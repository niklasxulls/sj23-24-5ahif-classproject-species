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

namespace DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Remove;
public class RemoveDangerousFeedCommand : IRequest, IMapFrom<DangerousFeed>
{
    public string DangerousFeedId { get; set; }
}

public class RemoveDangerousFeedCommandHandler : IRequestHandler<RemoveDangerousFeedCommand>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public RemoveDangerousFeedCommandHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RemoveDangerousFeedCommand request, CancellationToken cancellationToken)
    {
        var dangerousfeed = await _context.DangerousFeeds.Where(df => df.DangerousFeedExposedId == request.DangerousFeedId).FirstAsync(cancellationToken);

        _context.DangerousFeeds.Remove(dangerousfeed);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
