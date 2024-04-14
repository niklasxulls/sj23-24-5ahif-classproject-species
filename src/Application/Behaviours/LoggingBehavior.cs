using DiveSpecies.Application.Exceptions;
using DiveSpecies.Application.Interfaces.Services;
using DiveSpecies.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ILoggerService<TRequest> _logger;

    public LoggingBehavior(ILoggerService<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.Log("Logging Pre");
        _logger.Log(_logger.FormatPropsOfObj(request));
        try
        {
            return await next();
        }
        catch (Exception ex) { 
            _logger.Log(ex.Message, LoggingType.Error);
            if(!string.IsNullOrEmpty(ex.StackTrace))
            {
                _logger.Log(ex.StackTrace, LoggingType.Error);
            }

            throw;
        }
    }
}
