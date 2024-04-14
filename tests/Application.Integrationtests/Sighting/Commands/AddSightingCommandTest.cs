using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Xunit;

namespace Application.IntegrationTests.Questions.Commands;

public class AddSightingCommandTest : TestBase
{
    public AddSightingCommandTest(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldAddQuestion()
    {
        var firstSpecies = await _context.Species.FirstAsync();

        AddSightingCommand cmd = new AddSightingCommand
        {
            DiveExposedId = null,
            Sightings = new List<AddSighting>()
            {
                new()
                {
                    SpeciesExposedId = firstSpecies.SpeciesExposedId,
                    Count = 2,
                    Area = new MultiPolygon(new Polygon[]
                    {
                        Polygon.Empty
                    }),
                    DepthStartInMeter = 20,
                    DepthEndInMeter = 30,
                    DiveIntoInMinutes = 20
                }
            }
        };

        var diveExposedId = await SendMediator(cmd);


        var dive = _context.Dives.Where(u => u.DiveExposedId == diveExposedId).Include(d => d.Sightings).FirstOrDefault();

        dive.Should().NotBeNull();
        dive!.Sightings.Should().HaveCount(1);
        dive.Sightings.First().SpeciesId.Should().Be(firstSpecies.SpeciesId);
    }

}
