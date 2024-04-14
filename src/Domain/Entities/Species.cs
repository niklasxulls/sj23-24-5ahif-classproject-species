using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Domain.Entities;


public class Species
{
    public int SpeciesId { get; set; }
    public string SpeciesExposedId { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public int Population { get; set; }
    public SpeciesType SpeciesTypeId { get; set; }
    public SpeciesTypeType SpeciesType { get; set; } = null!;
    public WaterType OccuresIn { get; set; }
    public ICollection<SpeciesOccuresAt> OccuresAt { get; set; } = new HashSet<SpeciesOccuresAt>();


    public ICollection<DangerousFeed> DangerousFeeds { get; set; } = new HashSet<DangerousFeed>();
    public ICollection<SpeciesImage> SpeciesImages { get; set; }
}
