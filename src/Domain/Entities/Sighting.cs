using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Domain.Entities
{
    public class Sighting : BaseEntity
    {
        public int SightingId { get; set; }
        public string SightingExposedId { get; set; } = string.Empty;
        public int DiveId { get; set; }
        public Dive Dive { get; set; } = null!;
        public int SpeciesId { get; set; }
        public Species Species { get; set; } = null!;
        public double? DepthStartInMeter { get; set; }
        public double? DepthEndInMeter { get; set; }
        public double DiveIntoInMinutes { get; set; }
        public MultiPolygon Area { get; set; } = new MultiPolygon(new Polygon[] { });
        public int Count { get; set; }
        public ICollection<SightingImage> SightingImages { get; set; }
    }

}
