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
using DiveSpecies.Application.Models.DTOs.SpeciesNS;

namespace DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetDetails;
public class GetSpeciesDetailsQuery : IRequest<SpeciesDetailsDto>, IMapFrom<SpeciesDetailsDto>
{
    public string SpeciesId { get; set; }
}

public class GetSpeciesDetailsQueryHandler : IRequestHandler<GetSpeciesDetailsQuery, SpeciesDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public GetSpeciesDetailsQueryHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SpeciesDetailsDto> Handle(GetSpeciesDetailsQuery request, CancellationToken cancellationToken)
    {
        var species = await _context.Species.Where(s => s.SpeciesExposedId == request.SpeciesId)
                                            .ProjectTo<SpeciesDetailsDto>(_mapper.ConfigurationProvider)
                                            .FirstAsync(cancellationToken);

        return species;
    }
}
