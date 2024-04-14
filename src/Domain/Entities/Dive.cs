using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Domain.Entities;

public class Dive : BaseEntity
{
    public int DiveId { get; set; }
    public string DiveExposedId { get; set; } = string.Empty;

    public string UserExposedId { get; set; }

    public ICollection<Sighting> Sightings { get; set; } = new HashSet<Sighting>();
}
