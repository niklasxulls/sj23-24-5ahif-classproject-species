using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
public class UpdateSightingCommandValidator : AbstractValidator<UpdateSightingCommand>
{
    public UpdateSightingCommandValidator()
    {
    }
}
