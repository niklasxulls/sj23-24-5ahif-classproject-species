using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetDetails;

public class SearchSpeciesQueryValidator : AbstractValidator<SearchSpeciesQuery>
{
    public SearchSpeciesQueryValidator()
    {
    }
}
