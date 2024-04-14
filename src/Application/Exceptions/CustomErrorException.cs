using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Exceptions;

public class CustomErrorException : Exception
{
    public int ErrorCode { get; set; }
    public CustomErrorException(int errorCode, string message) :base(message)
    {
        ErrorCode = errorCode;
    }
}
