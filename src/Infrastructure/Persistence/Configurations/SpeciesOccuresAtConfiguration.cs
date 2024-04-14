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
    public class SpeciesOccuresAtConfiguration : IEntityTypeConfiguration<SpeciesOccuresAt>
    {
        public void Configure(EntityTypeBuilder<SpeciesOccuresAt> builder)
        {
            builder.HasKey(r => r.SpeciesOccuresAtId);
            builder.Property(r => r.SpeciesOccuresAtId).UseIdentityColumn();


            builder.HasOne(r => r.Species)
                   .WithMany(r => r.OccuresAt)
                   .HasForeignKey(q => q.SpeciesId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
