
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; private set; } = new Dictionary<string, string[]>();
        public ValidationException() : base("At least one validation error has occured") {}
        public ValidationException(string msg) : base(msg) { }

        public ValidationException(IEnumerable<ValidationFailure> errors) : this()
        {
            Errors = errors
                        .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                        .ToDictionary(e => e.Key, e => e.ToArray());
        }

        public ValidationException(IDictionary<string, string[]> errors)
        {
            this.Errors = errors;
        }


    }
}
