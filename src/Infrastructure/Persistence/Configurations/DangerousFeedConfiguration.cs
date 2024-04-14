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
    public class DangerousFeedConfiguration : IEntityTypeConfiguration<DangerousFeed>
    {
      
        public void Configure(EntityTypeBuilder<DangerousFeed> builder)
        {
            builder.HasKey(r => r.DangerousFeedId);
            builder.Property(r => r.DangerousFeedId).UseIdentityColumn();

            builder.Property(r => r.DangerousFeedExposedId)
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator<Base58ValueGenerator>();

            builder.HasIndex(r => r.DangerousFeedId).IsUnique();


            builder.Property(s => s.Headline).HasMaxLength(500);
            builder.Property(s => s.Body).HasMaxLength(5000);

            builder.HasMany(s => s.Species).WithMany(sj => sj.DangerousFeeds);
        }
    }
}
