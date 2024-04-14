using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Domain.Entities;
public class SpeciesImage
{
    public int SpeciesImageId { get; set; }

    public string Url { get; set; }
    
    public int SpeciesId { get; set; }

    public Species Species { get; set; }

}
