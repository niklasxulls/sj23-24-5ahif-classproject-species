using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using DiveSpecies.Application.Interfaces;
using DiveSpecies.Application.Interfaces.Services.Data;
using DiveSpecies.Domain.Entities;
using DiveSpecies.Infrastructure.Util;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Infrastructure.Services.Data;
public class FakeDataService : IFakeDataService
{
    private readonly IDiveSpeciesDbContext _context;

    public FakeDataService(IDiveSpeciesDbContext context)
    {
        _context = context;
    }

    public List<Species> GenerateSpeciesData(int count, List<SpeciesTypeType> speciesTypeData)
    {
        var speciesFaker = new Faker<Species>().Rules((f, a) =>
        {
            a.Name = f.Lorem.Word();
            a.SpeciesExposedId = Base58.randomString();
            a.SpeciesTypeId = f.PickRandom(speciesTypeData).SpeciesTypeId;
            a.OccuresIn = f.PickRandom<WaterType>();
            a.OccuresAt = GenerateSpeciesOccursAtData(1);
            a.Population = f.Random.Int(1, 150);
            a.SpeciesImages = new List<SpeciesImage>() { new SpeciesImage() { Url = f.Internet.Url() } };
        });

        return speciesFaker.Generate(count);
    }

    public (List<Dive> DiveData, List<Sighting> SightingData) GenerateDiveAndSightingData(int diveCount, int sightingCount, List<Species> speciesData)
    {
        var diveAndSightingData = new List<Dive>();
        var sightings = new List<Sighting>();

        for (int i = 0; i < diveCount; i++)
        {
            var dive = new Faker<Dive>()
                .RuleFor(f => f.Sightings, sightings)
                .RuleFor(f => f.UserExposedId, Base58.randomString())
                .RuleFor(f => f.DiveExposedId, Base58.randomString());

            diveAndSightingData.Add(dive);
            sightings.AddRange(GenerateSightingData(sightingCount / diveCount, dive, speciesData));
        }

        return (diveAndSightingData, sightings);
    }

    public List<Sighting> GenerateSightingData(int count, Dive dive, List<Species> speciesData)
    {
        var sightingFaker = new Faker<Sighting>()
            .RuleFor(f => f.DiveId, dive.DiveId)
            .RuleFor(f => f.SightingExposedId, Base58.randomString())
            .RuleFor(f => f.SpeciesId, f => f.PickRandom(speciesData).SpeciesId)
            .RuleFor(f => f.DepthStartInMeter, f => f.Random.Double(1, 50))
            .RuleFor(f => f.DepthEndInMeter, f => f.Random.Double(51, 100))
            .RuleFor(f => f.DiveIntoInMinutes, f => f.Random.Double(10, 60))
            .RuleFor(f => f.Area, f => MultiPolygon.Empty)
            .RuleFor(f => f.Count, f => f.Random.Int(1, 10));

        return sightingFaker.Generate(count);
    }

    public List<SpeciesOccuresAt> GenerateSpeciesOccursAtData(int count)
    {
        var speciesOccursAtData = new List<SpeciesOccuresAt>();

        var faker = new Faker<SpeciesOccuresAt>()
            .RuleFor(f => f.Area, GenerateEmptyPolygon())
            .RuleFor(f => f.DepthStartInMeter, f => f.Random.Double(1, 50))
            .RuleFor(f => f.DepthEndInMeter, f => f.Random.Double(51, 100));

        for (int i = 0; i < count; i++)
        {
            var occursAt = faker.Generate();
            speciesOccursAtData.Add(occursAt);
        }

        return speciesOccursAtData;
    }

    private Polygon GenerateEmptyPolygon()
    {
        var multiPolygon = Polygon.Empty;
        multiPolygon.SRID = 4326;

        return multiPolygon;
    }
}
