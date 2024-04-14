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
    public class SpeciesSearchHistoryConfiguration : IEntityTypeConfiguration<SpeciesSearchHistory>
    {
        public void Configure(EntityTypeBuilder<SpeciesSearchHistory> builder)
        {
            builder.HasKey(r => r.SpeciesSearchHistoryId);
            builder.Property(r => r.SpeciesSearchHistoryId).UseIdentityColumn();

            builder.Property(s => s.SpeciesTypeId).HasConversion<int>();
            builder.Property(s => s.OccuresIn).HasConversion<int>();
        }
    }
}
