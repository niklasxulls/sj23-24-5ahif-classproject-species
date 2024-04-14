using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using DiveSpecies.Application.Models.DTOs.SpeciesNS;
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Xunit;

namespace Application.IntegrationTests.SpeciesNS.Commands;

public class AddSpeciesCommandTest : TestBase
{
    public AddSpeciesCommandTest(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldAddSpecies()
    {
        AddSpeciesCommand cmd = new AddSpeciesCommand
        {
            Name = "Alaskian King Crab",
            OccuresAt = new List<SpeciesOccuresAtDto>()
            {
                new SpeciesOccuresAtDto()
                {
                    Area = Polygon.Empty,
                    DepthStartInMeter = 10,
                    DepthEndInMeter = 20,
                }
            },
            OccuresIn = WaterType.Both,
            SpeciesTypeId = SpeciesType.Crab
        };

        var speciesExposedId = await SendMediator(cmd);


        var species = _context.Species.Where(u => u.SpeciesExposedId == speciesExposedId).FirstOrDefault();

        species.Should().NotBeNull();
        species!.Name.Should().Be(cmd.Name);
        species!.OccuresIn.Should().Be(cmd.OccuresIn);
        species!.SpeciesTypeId.Should().Be(cmd.SpeciesTypeId);

    }

}
