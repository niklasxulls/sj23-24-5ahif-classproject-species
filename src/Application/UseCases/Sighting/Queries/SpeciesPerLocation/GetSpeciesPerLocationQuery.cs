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
using DiveSpecies.Domain.Entities;
using System.Diagnostics.Metrics;
using NetTopologySuite.Geometries;
using AutoMapper.QueryableExtensions;

namespace DiveSpecies.Application.UseCases.SightingNS.Queries.SpeciesPerLocation;
public class GetSpeciesPerLocationQuery : IRequest<ICollection<SpeciesPerLocationCluster>>
{
    public string? SpeciesId { get; set; }
    public double Proximity { get; set; }
}

public class GetSpeciesPerLocationCommandHandler : IRequestHandler<GetSpeciesPerLocationQuery, ICollection<SpeciesPerLocationCluster>>
{
    private readonly IMapper _mapper;
    private readonly IDiveSpeciesDbContext _context;

    public GetSpeciesPerLocationCommandHandler(IDiveSpeciesDbContext context, IMapper mapper)
    {
		_context = context;
		_mapper = mapper;
    }

    public async Task<ICollection<SpeciesPerLocationCluster>> Handle(GetSpeciesPerLocationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Sightings.AsQueryable();

        if(!string.IsNullOrEmpty(request.SpeciesId))
        {
            query = query.Where(s => s.Species.SpeciesExposedId == request.SpeciesId);
        }

        var sightings = await query.ProjectTo<SpeciesPerLocationDto>(_mapper.ConfigurationProvider)
                                   .ToListAsync(cancellationToken);

        var clusters = ClusterAreas(request, sightings);

        return clusters;
    }

    public List<SpeciesPerLocationCluster> ClusterAreas(GetSpeciesPerLocationQuery request, List<SpeciesPerLocationDto> sigthings)
    {
        var clusters = new List<SpeciesPerLocationCluster>();

        foreach (var sighting in sigthings)
        {
            bool addedToCluster = false;

            foreach (var cluster in clusters)
            {
                if (cluster.Species.Any(a => a.Area.Distance(sighting.Area) < request.Proximity))
                {
                    cluster.Species.Add(sighting);
                    addedToCluster = true;
                    break;
                }
            }

            if (!addedToCluster)
            {
                var newCluster = new SpeciesPerLocationCluster();
                clusters.Add(newCluster);

                newCluster.Species.Add(sighting);
                UpdateClusterCentroid(newCluster);
            }
        }
        return clusters;
    }

    private void UpdateClusterCentroid(SpeciesPerLocationCluster cluster)
    {
        var allPoints = cluster.Species.SelectMany(a => a.Area.Coordinates);

        var centroidX = allPoints.Average(p => p.X);
        var centroidY = allPoints.Average(p => p.Y);
        
        cluster.Centroid = new Point(centroidX, centroidY);
    }
}
