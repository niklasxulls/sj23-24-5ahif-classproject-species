using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException() : base("The entity already exists!")
    {

    }

    public AlreadyExistsException(string message) : base(message)
    {

    }
}
