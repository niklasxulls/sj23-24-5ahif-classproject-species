using DiveSpecies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Interfaces.Services
{
    public interface ILoggerService<T>
    {
        void Log(string message, LoggingType type = LoggingType.Information);
        string FormatPropsOfObj(T obj);
    }
}
