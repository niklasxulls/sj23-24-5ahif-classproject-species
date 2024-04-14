using DiveSpecies.Application.Behaviours;
using DiveSpecies.Application.Interfaces.Services;
using DiveSpecies.Application.Models;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using MediatR;
using Moq;
using Xunit;

namespace stackblob.Application.UnitTests.Common.Behaviours;

public class LoggerBehaviorTests
{
    private readonly Mock<ILoggerService<IRequest<AddSpeciesCommand>>> _logger ;
    private readonly LoggingBehavior<IRequest<AddSpeciesCommand>, AddSpeciesCommand> _sut;
    public LoggerBehaviorTests()
    {
        _logger = new Mock<ILoggerService<IRequest<AddSpeciesCommand>>>();
        _sut = new LoggingBehavior<IRequest<AddSpeciesCommand>, AddSpeciesCommand>(_logger.Object);

    }

    [Fact]
    public async Task ShouldLog()
    {
        await _sut.Handle(Mock.Of<IRequest<AddSpeciesCommand>>(), new CancellationToken(), Mock.Of<RequestHandlerDelegate<AddSpeciesCommand>>());

        _logger.Verify(i => i.Log(It.IsAny<string>(), It.IsAny<LoggingType>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ShouldLogError()
    {
        try
        {
            await _sut.Handle(Mock.Of<IRequest<AddSpeciesCommand>>(), new CancellationToken(),() => { return null!; });
        } catch(Exception ex)
        {

        }

        _logger.Verify(i => i.Log(It.IsAny<string>(), LoggingType.Error), Times.AtLeastOnce);
    }
}
