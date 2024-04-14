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

namespace DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
public class AddSightingCommand : IRequest<string>, IMapFrom<Sighting>
{
    public string? DiveExposedId { get; set; }
    public string UserExposedId { get; set; }

    public ICollection<AddSighting> Sightings { get; set; }
}

public class AddSightingCommandHandler : IRequestHandler<AddSightingCommand, string>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;
    private readonly IMediaService _mediaService;

    public AddSightingCommandHandler(IDiveSpeciesDbContext context, IMapper mapper, IMediaService mediaService)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> Handle(AddSightingCommand request, CancellationToken cancellationToken)
    {
        Dive? dive = null;

        if(!string.IsNullOrEmpty(request.DiveExposedId))
        {
            dive = await _context.Dives.Where(d => d.DiveExposedId == request.DiveExposedId).FirstOrDefaultAsync(cancellationToken);
        } else {
            dive = new Dive() { 
                UserExposedId = request.UserExposedId,
            };

            await _context.Dives.AddAsync(dive, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var sightings = request.Sightings.Select(s => _mapper.Map<Sighting>(s)).ToList();

        foreach (var sighting in sightings)
        {
            var rawSighting = request.Sightings.Where(a => a.SpeciesExposedId == sighting.Species.SpeciesExposedId).First();

            var speciesImagesUrls = await _mediaService.UploadMedia(rawSighting.Images);
            sighting.SightingImages = speciesImagesUrls.Select(i => new SightingImage() { Url = i }).ToList();

            dive.Sightings.Add(sighting);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return dive.DiveExposedId;
    }
}
