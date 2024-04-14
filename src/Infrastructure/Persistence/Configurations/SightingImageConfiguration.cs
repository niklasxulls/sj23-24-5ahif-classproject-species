using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiveSpecies.Infrastructure.Persistence.Configurations;
public class SightingImageConfiguration : IEntityTypeConfiguration<SightingImage>
{
    public void Configure(EntityTypeBuilder<SightingImage> builder)
    {
        builder.HasKey(i => i.SightingImageId);
        builder.Property(i => i.Url)
               .IsRequired();

        builder.HasOne(i => i.Sighting)
               .WithMany(s => s.SightingImages)
               .HasForeignKey(i => i.SightingId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
