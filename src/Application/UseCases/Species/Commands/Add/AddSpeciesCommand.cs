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
using DiveSpecies.Application.Models.DTOs.SpeciesNS;
using DiveSpecies.Application.Interfaces.Services.Data;

namespace DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
public class AddSpeciesCommand : IRequest<string>, IMapFrom<Species>
{
    public string Name { get; set; }
    public string WikipediaUrl { get; set; }
    public WaterType OccuresIn { get; set; }
    public SpeciesType SpeciesTypeId { get; set; }
    public IFormFileCollection Images { get; set; }
    public ICollection<SpeciesOccuresAtDto> OccuresAt { get; set; }
}

public class AddSpeciesCommandHandler : IRequestHandler<AddSpeciesCommand, string>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;
    private readonly IMediaService _mediaService;

    public AddSpeciesCommandHandler(IDiveSpeciesDbContext context, IMapper mapper, IMediaService mediaService)
    {
        _context = context;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    public async Task<string> Handle(AddSpeciesCommand request, CancellationToken cancellationToken)
    {
        var species = _mapper.Map<Species>(request);

        var speciesImagesUrls = await _mediaService.UploadMedia(request.Images);
        species.SpeciesImages = speciesImagesUrls.Select(url => new SpeciesImage() { Url = url }).ToList();

        await _context.Species.AddAsync(species, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        return species.SpeciesExposedId;
    }
}
