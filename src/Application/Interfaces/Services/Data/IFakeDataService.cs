using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Domain.Entities;

namespace DiveSpecies.Application.Interfaces.Services.Data;
public interface IFakeDataService
{

    List<Species> GenerateSpeciesData(int count, List<SpeciesTypeType> speciesTypeData);

    (List<Dive> DiveData, List<Sighting> SightingData) GenerateDiveAndSightingData(int diveCount, int sightingCount, List<Species> speciesData);

    List<Sighting> GenerateSightingData(int count, Dive dive, List<Species> speciesData);

    List<SpeciesOccuresAt> GenerateSpeciesOccursAtData(int count);
}
