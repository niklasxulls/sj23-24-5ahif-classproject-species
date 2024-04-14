using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Infrastructure.Persistence.Configurations.Generator;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using DiveSpecies.Infrastructure.Persistence.Configurations.Converter;

namespace DiveSpecies.Infrastructure.Persistence.Configurations;
public class SightingConfiguration : IEntityTypeConfiguration<Sighting>
{
    public void Configure(EntityTypeBuilder<Sighting> builder)
    {
        builder.HasKey(r => r.SightingId);
        builder.Property(r => r.SightingId).UseIdentityColumn();

        builder.Property(r => r.SightingExposedId)
               .ValueGeneratedOnAdd()
               .HasValueGenerator<Base58ValueGenerator>();

        builder.HasIndex(r => r.SightingExposedId).IsUnique();

        builder.Property(s => s.Area);

        builder.HasOne(a => a.Dive)
               .WithMany(a => a.Sightings)
               .HasForeignKey(a => a.DiveId);

        builder.HasMany(r => r.SightingImages)
               .WithOne(r => r.Sighting)
               .HasForeignKey(r => r.SightingId);
    }
}
