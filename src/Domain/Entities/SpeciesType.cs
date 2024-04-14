using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Domain.Entities;
public class SpeciesTypeType : BaseEntityEnum
{
    public SpeciesType SpeciesTypeId { get; set; }
    
    public ICollection<Species> Species { get; set; } =  new HashSet<Species>();
}


public enum SpeciesType
{
    Crab = 1,
    Fish = 2,
    Turtle = 3,
}
