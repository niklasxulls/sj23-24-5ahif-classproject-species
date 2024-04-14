using DiveSpecies.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using System.Runtime.CompilerServices;
using DiveSpecies.Application.Interfaces.Services.Data;
using NetTopologySuite.Geometries;

namespace DiveSpecies.Infrastructure.Persistence;

public static class DiveSpeciesDbContextSeed
{

    public static async Task SeedSampleData(DiveSpeciesDbContext context)
    {
        if (context.SpeciesTypes.Count() == 0)
        {
            context.SpeciesTypes.Add(new SpeciesTypeType { SpeciesTypeId = SpeciesType.Fish, Name = "Fish", Desc = "Fish" });
            context.SpeciesTypes.Add(new SpeciesTypeType { SpeciesTypeId = SpeciesType.Turtle, Name = "Turtle", Desc = "Turtle" });
            context.SpeciesTypes.Add(new SpeciesTypeType { SpeciesTypeId = SpeciesType.Crab, Name = "Crab", Desc = "Crab" });
        }

        await context.SaveChangesAsync(default);
    }

    public static async Task TryAddSpeciesFromThirdPartyToDB(DiveSpeciesDbContext context, IThirdPartyAPIService thirdPartyAPIService, IFakeDataService fakeDataService)
    {
        if (context.Species.Count() == 0)
        {
            Console.WriteLine("checked species cnt");

            var fishData = await thirdPartyAPIService.RequestAllFishData();
            
            Console.WriteLine("fetched data");

            foreach (var fish in fishData)
            {
                var species = fakeDataService.GenerateSpeciesData(1, context.SpeciesTypes.ToList()).First();
                Console.WriteLine("generated species data");

                species.Name = fish.Name;

                species.SpeciesImages = new List<SpeciesImage>()
                {
                    new SpeciesImage
                    {
                        Url = fish.ImgSrcSetTwoX ?? "default_url_if_null" // coalesce value
                    }
                };


                foreach (var a in species.OccuresAt)
                {
                    a.Area = null;
                }

                context.Species.Add(species);
            
                Console.WriteLine("added species");
            }

            await context.SaveChangesAsync(default);
        }
    }

}
