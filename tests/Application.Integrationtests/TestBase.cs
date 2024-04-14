using AutoMapper;
using Bogus;
using DiveSpecies.Application.Interfaces.Services.Data;
using DiveSpecies.Domain.Entities;
using DiveSpecies.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTests
{
    [Collection("global")]
    public class TestBase : IAsyncLifetime
    {
        private readonly SetupFixture _setup;
        protected readonly IServiceScope _scope;
        protected readonly DiveSpeciesDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IFakeDataService _fakeDataService;

        public TestBase(SetupFixture setup)
        {
            _setup = setup;
            _scope = _setup._scopeFactory.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<DiveSpeciesDbContext>();
            _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
            _fakeDataService = _scope.ServiceProvider.GetRequiredService<IFakeDataService>();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public async Task<TResponse> SendMediator<TResponse>(IRequest<TResponse> request)
        {
            var mediator = _scope.ServiceProvider.GetRequiredService<ISender>();
            return await mediator.Send(request);
        }

        private async Task FillDB()
        {
            await DiveSpeciesDbContextSeed.SeedSampleData(_context);

            var speciesData = _fakeDataService.GenerateSpeciesData(6, _context.SpeciesTypes.ToList());
            _context.Species.AddRange(speciesData);

            var diveAndSightingData = _fakeDataService.GenerateDiveAndSightingData(3, 7, speciesData);
            _context.Dives.AddRange(diveAndSightingData.DiveData);
            _context.Sightings.AddRange(diveAndSightingData.SightingData);

            _context.SaveChanges();
        }

        public async Task InitializeAsync()
        {
            await FillDB();
        }

        public async Task DisposeAsync()
        {
            _scope.Dispose();
            await Task.FromResult(0);
        }
    }
}
