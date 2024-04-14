using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Domain.Entities
{
    public class DangerousFeed
    {
        public int DangerousFeedId { get; set; }
        public string DangerousFeedExposedId { get; set; } = null!;

        public string Headline { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public DateTime OccuresAtFrom { get; set; }
        public DateTime OccuresAtTill { get; set; }

        public double? DepthStartInMeter { get; set; }
        public double? DepthEndInMeter { get; set; }
        public MultiPolygon? Area { get; set; }
        
        public ICollection<Species> Species { get; set; } = new HashSet<Species>();
    }
}
