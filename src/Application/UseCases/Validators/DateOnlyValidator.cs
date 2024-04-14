using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Domain.Util;
using FluentValidation;
using FluentValidation.Validators;

namespace DiveSpecies.Application.UseCases.Validators;

public class DateOnlyValidator<T> : IPropertyValidator<T, DateOnly>
{
    private readonly bool _required;

    public DateOnlyValidator(bool required = true)
    {
        _required = required;
    }
    public string Name => "DateOnly Validator";

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "The date is not valid";
    }

    public bool IsValid(ValidationContext<T> context, DateOnly value)
    {
        return DateTimeUtil.IsValid(value.ToDateTime());
    }
}
