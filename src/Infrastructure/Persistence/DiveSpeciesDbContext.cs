using DiveSpecies.Application.Interfaces;
using DiveSpecies.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Application.Interfaces.Services;
using DiveSpecies.Domain.Util;

namespace DiveSpecies.Infrastructure.Persistence;

public class DiveSpeciesDbContext : DbContext, IDiveSpeciesDbContext
{
    public DbSet<Species> Species { get; set; } = null!;
    public DbSet<SpeciesTypeType> SpeciesTypes { get; set; }
    //public DbSet<SearchHistory> SpeciesSearchHistory { get; set; }
    public DbSet<Dive> Dives { get; set; }
    public DbSet<Sighting> Sightings { get; set; }

    public DbSet<DangerousFeed> DangerousFeeds {get; set;}
    public DbSet<SpeciesSearchHistory> SpeciesSearchHistory { get; set; }

    public DiveSpeciesDbContext(DbContextOptions<DiveSpeciesDbContext> options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTimeUtil.Now();
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTimeUtil.Now();
                entry.State = EntityState.Modified;
            }
        
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        builder.ApplyConfigurationsFromAssembly(assembly);


        base.OnModelCreating(builder);
    }
}
