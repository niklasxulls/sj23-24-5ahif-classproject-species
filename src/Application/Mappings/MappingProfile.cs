
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)
        )).ToList();

        types.ForEach(t =>
        {
            var instance = Activator.CreateInstance(t);

            if(t.GetMethod("Mapping") != null)
            {
                t.GetMethod("Mapping")?.Invoke(instance, new object[] { this });
            } else
            {
                t.GetInterfaces().Where(i => i.Name.Equals("IMapFrom`1"))?.ToList()?.ForEach(i =>
                {
                    i.GetMethod("Mapping")?.Invoke(instance, new object[] { this });
                });
            }
        });

       

    }

}
