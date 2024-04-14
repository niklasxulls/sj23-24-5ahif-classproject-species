using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using DiveSpecies.Application.Interfaces;

namespace DiveSpecies.Application.UseCases.Validators;

public class IFormFileCollectionValidator<T> : IPropertyValidator<T, ICollection<IFormFile>>
{
    private readonly bool _required;

    public IFormFileCollectionValidator(bool required = true)
    {
        _required = required;
    }
    public string Name => "Formfile Collection Validator";

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "The iform collection is either null, of length 0 or one of the provided iforms doesnt match the requirements";
    }

    public bool IsValid(ValidationContext<T> context, ICollection<IFormFile> value)
    {
        if (value == null || value.Count == 0) return !_required;

        foreach(var file in value)
        {
            if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file.Name) || string.IsNullOrEmpty(file.ContentType)) return false;
        }

        return true;
    }
}
