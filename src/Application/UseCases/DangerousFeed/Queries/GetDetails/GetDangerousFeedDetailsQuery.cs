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
using DiveSpecies.Application.UseCases.DangerousFeedNS.Queries.GetDetails;

namespace DiveSpecies.Application.UseCases.DangerousFeedNS.Queries.GetDetails;
public class GetDangerousFeedDetailsQuery : IRequest<DangerousFeedDetailsDto>, IMapFrom<DangerousFeedDetailsDto>
{
    public string DangerousFeedId { get; set; }
}

public class GetDangerousFeedDetailsQueryHandler : IRequestHandler<GetDangerousFeedDetailsQuery, DangerousFeedDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public GetDangerousFeedDetailsQueryHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DangerousFeedDetailsDto> Handle(GetDangerousFeedDetailsQuery request, CancellationToken cancellationToken)
    {
        var dangerousfeed = await _context.DangerousFeeds.Where(df => df.DangerousFeedExposedId == request.DangerousFeedId)
                                            .ProjectTo<DangerousFeedDetailsDto>(_mapper.ConfigurationProvider)
                                            .FirstAsync(cancellationToken);

        return dangerousfeed;
    }
}
