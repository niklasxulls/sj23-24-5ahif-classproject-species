using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiveSpecies.Infrastructure.Persistence.Configurations.Lookup;


public class SpeciesTypeConfiguration : BaseEnumConfiguration<SpeciesTypeType>, IEntityTypeConfiguration<SpeciesTypeType>
{
    public override void Configure(EntityTypeBuilder<SpeciesTypeType> builder)
    {
        base.Configure(builder);

        builder.HasKey(r => r.SpeciesTypeId);
        builder.Property(r => r.SpeciesTypeId).HasConversion<int>();
    }
}
