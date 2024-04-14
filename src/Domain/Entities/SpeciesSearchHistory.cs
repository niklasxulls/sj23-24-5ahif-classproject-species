using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Domain.Entities
{
    public class SpeciesSearchHistory
    {
        public int SpeciesSearchHistoryId { get; set; }
        public string UserExposedId { get; set; } = string.Empty;


        public string SearchTerm { get; set; } = string.Empty;
        public SpeciesType? SpeciesTypeId { get; set; }
      

        public WaterType? OccuresIn { get; set; }
        public MultiPolygon? Area { get; set; }
        public int? Population { get; set; }
        public NumberParameterOperator? PopulationOperator { get; set; }
        public double? DepthStartInMeter { get; set; }
        public double? DepthEndInMeter { get; set; }


        public SpeciesSearchSortBy SortBy { get; set; }
        public bool SortDesc { get; set; }
    }
}
