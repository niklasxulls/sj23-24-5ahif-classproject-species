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
using DiveSpecies.Application.Models.DTOs.SpeciesNS;
using Microsoft.AspNetCore.Http;
using DiveSpecies.Application.Interfaces.Services.Data;

namespace DiveSpecies.Application.UseCases.SpeciesNS.Commands.Update;
public class UpdateSpeciesCommand : IRequest<string>, IMapFrom<Species>
{
    public string SpeciesId { get; set; }
    public string Name { get; set; }
    public WaterType OccuresIn { get; set; }
    public SpeciesType SpeciesTypeId { get; set; }
    public ICollection<SpeciesOccuresAtDto> OccuresAt { get; set; } 
    public MultiPolygon OccuresAtLocation { get; set; }
    public IFormFileCollection Images { get; set; }
}

public class UpdateSpeciesCommandHandler : IRequestHandler<UpdateSpeciesCommand, string>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;
    private readonly IMediaService _mediaService;

    public UpdateSpeciesCommandHandler(IDiveSpeciesDbContext context, IMapper mapper, IMediaService mediaService)
    {
        _context = context;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    public async Task<string> Handle(UpdateSpeciesCommand request, CancellationToken cancellationToken)
    {
        var species = await _context.Species.Where(s => s.SpeciesExposedId == request.SpeciesId).FirstAsync(cancellationToken);

        species.Name = request.Name;
        species.OccuresIn = request.OccuresIn;
        species.OccuresAt = new List<SpeciesOccuresAt>();

        var speciesImagesUrls = await _mediaService.UploadMedia(request.Images);
        species.SpeciesImages = speciesImagesUrls.Select(url => new SpeciesImage() { Url = url }).ToList();

        foreach(var occurence in request.OccuresAt)
        {
            var occurenceMapped = _mapper.Map<SpeciesOccuresAt>(occurence);
            occurenceMapped.SpeciesId = species.SpeciesId;

            species.OccuresAt.Add(occurenceMapped);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return species.SpeciesExposedId;
    }
}
