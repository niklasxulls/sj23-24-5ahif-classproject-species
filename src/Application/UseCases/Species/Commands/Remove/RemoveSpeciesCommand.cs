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

namespace DiveSpecies.Application.UseCases.SpeciesNS.Commands.Remove;
public class RemoveSpeciesCommand : IRequest, IMapFrom<Species>
{
    public string SpeciesId { get; set; }
}

public class RemoveSpeciesCommandHandler : IRequestHandler<RemoveSpeciesCommand>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public RemoveSpeciesCommandHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RemoveSpeciesCommand request, CancellationToken cancellationToken)
    {
        var species = await _context.Species.Where(s => s.SpeciesExposedId == request.SpeciesId).FirstAsync(cancellationToken);

        _context.Species.Remove(species);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
