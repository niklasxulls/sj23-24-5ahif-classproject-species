using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Add;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Update;
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Xunit;

namespace Application.IntegrationTests.DangerousFeedNS.Commands;

public class UpdateDangerousFeedCommandTest : TestBase
{
    public UpdateDangerousFeedCommandTest(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldUpdateDangerousFeed()
    {
        var speciesIdsBefore = await _context.Species.Take(2).Select(a => a.SpeciesExposedId).ToListAsync();

        AddDangerousFeedCommand cmd = new AddDangerousFeedCommand
        {

            Headline  = "DonauGefahr",
            Body = "Gefahr in der Donau",

            OccuresAtFrom = new DateTime(),
            OccuresAtTill = new DateTime(),

            DepthStartInMeter = 2,
            DepthEndInMeter = 5,
            Area = new MultiPolygon(new Polygon[] {Polygon.Empty }),

            SpeciesIds = speciesIdsBefore
        };

        var dangerousFeedExposedId = await SendMediator(cmd);
        var dangerousFeedBefore = _context.DangerousFeeds.Where(u => u.DangerousFeedExposedId == dangerousFeedExposedId).FirstOrDefault();


        var speciesIdsAfter = await _context.Species.Skip(3).Take(2).Select(a => a.SpeciesExposedId).ToListAsync();

        UpdateDangerousFeedCommand cmdUpdated = new UpdateDangerousFeedCommand{

            DangerousFeedId = dangerousFeedExposedId,
            Headline  = "DonauGefahr UPDATED",
            Body = "Gefahr in der Donau",

            OccuredAtFrom = new DateTime(),
            OccuredAtTill = new DateTime(),

            DepthStartInMeter = 3,
            DepthEndInMeter = 6,

            Area = new MultiPolygon(new Polygon[] {Polygon.Empty }),

            SpeciesIds = speciesIdsAfter
        };

        var dangerousFeedAfter = _context.DangerousFeeds.Where(u => u.DangerousFeedExposedId == dangerousFeedExposedId).FirstOrDefault();


        dangerousFeedAfter.Should().NotBeNull();

        dangerousFeedAfter!.Headline.Should().Be(cmdUpdated.Headline);
        dangerousFeedAfter!.Body.Should().Be(cmdUpdated.Body);

        dangerousFeedAfter!.OccuresAtFrom.Should().Be(cmdUpdated.OccuredAtFrom);
        dangerousFeedAfter!.OccuresAtTill.Should().Be(cmdUpdated.OccuredAtTill);

        dangerousFeedAfter!.DepthStartInMeter.Should().Be(cmdUpdated.DepthStartInMeter);
        dangerousFeedAfter!.DepthEndInMeter.Should().Be(cmdUpdated.DepthEndInMeter);

        dangerousFeedAfter!.Species.Should().HaveCount(speciesIdsAfter.Count);
        dangerousFeedAfter.Species.Select(a => a.SpeciesExposedId).Should().BeSameAs(speciesIdsAfter);
    }

}
