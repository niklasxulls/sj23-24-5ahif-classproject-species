using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UnitTests.Validators;
using DiveSpecies.Application.Interfaces;
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Infrastructure.Persistence.Configurations;
using FluentAssertions;
using Moq;
using Xunit;

namespace stackblob.Application.UnitTests.Validators;

public class AnswerValidatorTests
{
    private readonly Mock<IDiveSpeciesDbContext> _context;


    public AnswerValidatorTests()
    {
        _context = new Mock<IDiveSpeciesDbContext>();
    }

    [Fact]
    public void AddSightingCommandValidator_Should_Match_Configuration()
    {
        //var validator = new AddSightingCommandValidator();
        //ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<AddSighting, SightingConfiguration, AddSightingCommandValidator, AddSightingCommand>(validator);
    }
}

