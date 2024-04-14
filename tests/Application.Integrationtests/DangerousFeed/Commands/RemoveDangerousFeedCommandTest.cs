using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Add;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Remove;
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Xunit;

namespace Application.IntegrationTests.DangerousFeedNS.Commands;

public class RemoveDangerousFeedCommandTest : TestBase
{
    public RemoveDangerousFeedCommandTest(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShoulRemoveDangerousFeed()
    {
        var speciesIds = await _context.Species.Take(2).Select(a => a.SpeciesExposedId).ToListAsync();

        AddDangerousFeedCommand cmd = new AddDangerousFeedCommand
        {

            Headline  = "NeusiedlerseeGefahr",
            Body = "Gefahr im Neusiedlersee",

            OccuresAtFrom = new DateTime(),
            OccuresAtTill = new DateTime(),

            DepthStartInMeter = 2,
            DepthEndInMeter = 5,
            
            Area = new MultiPolygon(new Polygon[] {Polygon.Empty }),

            SpeciesIds = speciesIds
        };

        var dangerousFeedExposedId = await SendMediator(cmd);
        
       
       RemoveDangerousFeedCommand cmd2 = new RemoveDangerousFeedCommand{

            DangerousFeedId = dangerousFeedExposedId
       };

        var dangerousfeed = _context.DangerousFeeds.Where(u => u.DangerousFeedExposedId == dangerousFeedExposedId).FirstOrDefault();

        dangerousfeed.Should().BeNull();

    }

}
