using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
public class AddSightingCommandValidator : AbstractValidator<RemoveSightingCommand>
{
    public AddSightingCommandValidator()
    {
    }
}
