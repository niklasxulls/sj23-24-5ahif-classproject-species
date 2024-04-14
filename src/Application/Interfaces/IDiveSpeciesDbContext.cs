using DiveSpecies.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DiveSpecies.Application.Interfaces
{
    public interface IDiveSpeciesDbContext
    {
        DbSet<Species> Species { get; set; }
        DbSet<SpeciesTypeType> SpeciesTypes { get; set; }
        DbSet<SpeciesSearchHistory> SpeciesSearchHistory { get; set; }
        DbSet<Dive> Dives { get; set; }
        DbSet<Sighting> Sightings { get; set; }

        DbSet<DangerousFeed> DangerousFeeds { get; set;}

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    }
}
