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

namespace DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
public class RemoveSightingCommand : IRequest
{
    public string DiveExposedId { get; set; }
    public ICollection<string> SightingExposedIds { get; set; }
}

public class RemoveSightingCommandHandler : IRequestHandler<RemoveSightingCommand>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public RemoveSightingCommandHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RemoveSightingCommand request, CancellationToken cancellationToken)
    {
        var dive = await _context.Dives.Where(d => d.DiveExposedId == request.DiveExposedId)
                                       .Include(a => a.Sightings)
                                       .FirstAsync(cancellationToken);
        
        foreach(var sightingExposedId in request.SightingExposedIds)
        {
            var sighting = dive.Sightings.Where(a => a.SightingExposedId == sightingExposedId).FirstOrDefault();

            if (sighting == null)
                continue;

            _context.Sightings.Remove(sighting);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
