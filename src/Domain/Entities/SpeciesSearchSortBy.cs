using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Domain.Entities;


public enum SpeciesSearchSortBy
{
    Population,
    Latest,
    Name,
}

public enum SortDirection
{
    Ascending,
    Descending
}
