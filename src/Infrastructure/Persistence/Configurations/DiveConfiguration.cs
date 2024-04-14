using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Infrastructure.Persistence.Configurations.Generator;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DiveSpecies.Infrastructure.Persistence.Configurations;
public class DiveConfiguration : IEntityTypeConfiguration<Dive>
{
    public void Configure(EntityTypeBuilder<Dive> builder)
    {
        builder.HasKey(r => r.DiveId);
        builder.Property(r => r.DiveId).UseIdentityColumn();

        builder.Property(r => r.DiveExposedId)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<Base58ValueGenerator>();
    }
}
