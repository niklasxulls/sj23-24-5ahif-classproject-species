using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base("User has entered the wrong credentials/hasn't enough permissions for this action")
        {

        }

        public ForbiddenAccessException(string message) : base(message)
        {

        }
    }
}
