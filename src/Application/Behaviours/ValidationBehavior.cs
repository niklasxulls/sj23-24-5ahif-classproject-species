
using DiveSpecies.Application.Interfaces.Services;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Behaviours;

//basically this says that every Handler that has an request handler of the form <IRequest<T>, T>
//inheritas from IPipelineBehavior
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILoggerService<TRequest> _logger;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILoggerService<TRequest> logger)
    {
        this._validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        //pre
        _logger.Log("Validation Pre");
        if(_validators?.Any() ?? false)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var errors = validationResults.Where(v => v.Errors?.Any() ?? false).SelectMany(r => r.Errors).ToList();

            if(errors?.Any() ?? false)
            {
                throw new Exceptions.ValidationException(errors);
            }
        }
        var res = await next();
        //post
        _logger.Log("Validation Post");
        return res;
    }
}
