using DiveSpecies.Application.Interfaces.Services;
using DiveSpecies.Application.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Infrastructure.Services
{
    public class LoggerService<T> : ILoggerService<T>
    {
        private ILogger _logger;

        public LoggerService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(T).Name);
        }
        public void Log(string message, LoggingType type)
        {
            switch(type)
            {
                case LoggingType.Error: _logger.LogError(message); break;
                case LoggingType.Information: _logger.LogInformation(message); break;
                case LoggingType.Warning: _logger.LogWarning(message); break;
            }
        }

        public string FormatPropsOfObj(T obj)
        {
            if (obj is null) return "";

            var props = obj.GetType().GetProperties();
            StringBuilder sb = new StringBuilder(obj.GetType().Name + "[");
            for (int i=0; i<props.Length; i++)
            {
                if(i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append($"{props[i].Name}={props[i].GetValue(obj, null)}");
            }
            sb.Append("]");
            return sb.ToString();
        }

    }
}
