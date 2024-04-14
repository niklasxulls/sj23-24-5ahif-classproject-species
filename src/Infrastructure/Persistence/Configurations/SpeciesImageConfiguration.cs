using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiveSpecies.Infrastructure.Persistence.Configurations;
public class SpeciesImageConfiguration : IEntityTypeConfiguration<SpeciesImage>
{
    public void Configure(EntityTypeBuilder<SpeciesImage> builder)
    {
        builder.HasKey(i => i.SpeciesImageId);



        builder.Property(i => i.Url)
               .IsRequired();

        builder.HasOne(i => i.Species)
               .WithMany(s => s.SpeciesImages)
               .HasForeignKey(i => i.SpeciesId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
