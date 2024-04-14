using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile p) => p.CreateMap(typeof(T), GetType()).ReverseMap();
    }
}
