using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using DiveSpecies.Application.Models;

namespace DiveSpecies.Application.UseCases.Validators;

public class PagingValidator<T> : IPropertyValidator<T, Paging>
{
    private readonly bool _required;

    public PagingValidator(bool required = true)
    {
        _required = required;
    }
    public string Name => "Paging Validator";

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "The paging object is either null or not valid";
    }

    public bool IsValid(ValidationContext<T> context, Paging value)
    {
        if (value == null) return !_required;

        return value.Size <= 1000;
    }
}
