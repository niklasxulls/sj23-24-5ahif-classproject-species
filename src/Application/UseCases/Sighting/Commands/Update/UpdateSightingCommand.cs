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
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using Microsoft.AspNetCore.Http;
using DiveSpecies.Application.Interfaces.Services.Data;

namespace DiveSpecies.Application.UseCases.SightingNS.Commands.Add;

public class UpdateSightingCommand : IRequest
{
    public string DiveExposedId { get; set; }

    public ICollection<UpdateSighting> Sightings { get; set; }

    public IFormFileCollection Images { get; set; }

}

public class UpdateSightingCommandHandler : IRequestHandler<UpdateSightingCommand>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;
    private readonly IMediaService _mediaService;

    public UpdateSightingCommandHandler(IDiveSpeciesDbContext context, IMapper mapper, IMediaService mediaService)
    {
        _context = context;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    public async Task<Unit> Handle(UpdateSightingCommand request, CancellationToken cancellationToken)
    {
        var dive = await _context.Dives.Where(d => d.DiveExposedId == request.DiveExposedId)
                                       .Include(a => a.Sightings)
                                       .FirstAsync(cancellationToken);


        var sightings = request.Sightings.Select(s => _mapper.Map<Sighting>(s)).ToList();

        if (sightings?.Any() ?? false)
        {
            foreach (var sighting in sightings)
            {
                var matchingSighting = dive.Sightings.FirstOrDefault(e => e.SightingExposedId == sighting.SightingExposedId);

                if (matchingSighting == null)
                {
                    sighting.SightingExposedId = null!;

                    await _context.Sightings.AddAsync(sighting, cancellationToken);
                }
                else
                {
                    matchingSighting.DiveIntoInMinutes = sighting.DiveIntoInMinutes;
                    matchingSighting.Count = sighting.Count;
                    matchingSighting.Area = sighting.Area;
                    matchingSighting.DepthEndInMeter = sighting.DepthEndInMeter;
                    matchingSighting.DepthEndInMeter = sighting.DepthEndInMeter;

                    if(request.Images.Count > 0)
                    {
                        var speciesImagesUrls = await _mediaService.UploadMedia(request.Images);
                        matchingSighting.SightingImages = speciesImagesUrls.Select(url => new SightingImage { Url = url }).ToList();
                    }

                }
            }

            var sightingsToRemove = dive.Sightings.Where(e => !request.Sightings.Any(ei => ei.SightingExposedId == e.SightingExposedId)).ToList();

            foreach (var sighting in sightingsToRemove)
            {
                dive.Sightings.Remove(sighting);
            }
        }
        else
        {
            _context.Sightings.RemoveRange(dive.Sightings);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
