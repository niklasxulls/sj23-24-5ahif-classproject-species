using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Add;
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Xunit;

namespace Application.IntegrationTests.DangerousFeedNS.Commands;

public class AddDangerousFeedCommandTest : TestBase
{
    public AddDangerousFeedCommandTest(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldAddDangerousFeed()
    {
        var speciesIds = await _context.Species.Take(2).Select(a => a.SpeciesExposedId).ToListAsync();

        AddDangerousFeedCommand cmd = new AddDangerousFeedCommand
        {

            Headline = "BodenseeGefahr",
            Body = "Gefahr im Bodensee",

            OccuresAtFrom = new DateTime(),
            OccuresAtTill = new DateTime(),

            DepthStartInMeter = 2,
            DepthEndInMeter = 5,
            Area = new MultiPolygon(new Polygon[] { Polygon.Empty }),
            SpeciesIds = speciesIds
        };

        var dangerousFeedExposedId = await SendMediator(cmd);


        var dangerousFeed = _context.DangerousFeeds.Where(u => u.DangerousFeedExposedId == dangerousFeedExposedId).FirstOrDefault();

        dangerousFeed.Should().NotBeNull();

        dangerousFeed!.Headline.Should().Be(cmd.Headline);
        dangerousFeed!.Body.Should().Be(cmd.Body);

        dangerousFeed!.OccuresAtFrom.Should().Be(cmd.OccuresAtFrom);
        dangerousFeed!.OccuresAtTill.Should().Be(cmd.OccuresAtTill);

        dangerousFeed!.DepthStartInMeter.Should().Be(cmd.DepthStartInMeter);
        dangerousFeed!.DepthEndInMeter.Should().Be(cmd.DepthEndInMeter);
        
        dangerousFeed!.Species.Should().HaveCount(speciesIds.Count);
        dangerousFeed.Species.Select(a => a.SpeciesExposedId).Should().BeSameAs(speciesIds);
    }

}
