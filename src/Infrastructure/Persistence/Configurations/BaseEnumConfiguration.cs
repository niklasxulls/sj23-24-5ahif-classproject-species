using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiveSpecies.Infrastructure.Persistence.Configurations;
public class BaseEnumConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntityEnum
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Desc).IsRequired().HasMaxLength(500);
    }
}
