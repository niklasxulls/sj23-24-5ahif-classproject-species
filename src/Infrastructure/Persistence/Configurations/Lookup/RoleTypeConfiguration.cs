using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiveSpecies.Infrastructure.Persistence.Configurations.Lookup;

//public class RoleTypeConfiguration : BaseEnumConfiguration<RoleTypeType>, IEntityTypeConfiguration<RoleTypeType>
//{
//    public override void Configure(EntityTypeBuilder<RoleTypeType> builder)
//    {
//        base.Configure(builder);

//        builder.HasKey(r => r.RoleTypeId);
//        builder.Property(r => r.RoleTypeId).HasConversion<int>();
//    }
//}
