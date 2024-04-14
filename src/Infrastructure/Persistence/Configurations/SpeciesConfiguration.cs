using DiveSpecies.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using DiveSpecies.Infrastructure.Persistence.Configurations.Generator;
using System.Text.Json;
using DiveSpecies.Infrastructure.Persistence.Configurations.Converter;

namespace DiveSpecies.Infrastructure.Persistence.Configurations
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.HasKey(r => r.SpeciesId);
            builder.Property(r => r.SpeciesId).UseIdentityColumn();

            builder.Property(r => r.SpeciesExposedId)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<Base58ValueGenerator>();

            builder.HasIndex(r => r.SpeciesId).IsUnique();
            
            
            builder.Property(s => s.Name).HasMaxLength(200);

            builder.Property(s => s.SpeciesTypeId).HasConversion<int>();
            builder.Property(s => s.OccuresIn).HasConversion<int>();


            builder.HasOne(r => r.SpeciesType)
                   .WithMany(r => r.Species)
                   .HasForeignKey(q => q.SpeciesTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.SpeciesImages)
                   .WithOne(r => r.Species)
                   .HasForeignKey(r => r.SpeciesId);
        }
    }
}
