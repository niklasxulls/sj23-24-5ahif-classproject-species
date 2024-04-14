using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Application.Interfaces;
using DiveSpecies.Application.Mapping;
using DiveSpecies.Domain.Entities;
using NetTopologySuite.Geometries;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetDetails;

namespace DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetLatestSearch;

public class SpeciesSearchHistoryDto : IMapFrom<SpeciesSearchHistory>
{
    public string SearchTerm { get; set; } = string.Empty;
    public SpeciesType? SpeciesType { get; set; }
    public int? SpeciesTypeId { get; set; }


    public WaterType? OccuresIn { get; set; }
    public MultiPolygon OccuresAtLocation { get; set; }
    public int? Population { get; set; }
    public NumberParameterOperator? PopulationOperator { get; set; }
    public double? DepthStartInMeter { get; set; }
    public double? DepthEndInMeter { get; set; }


    public SpeciesSearchSortBy SortBy { get; set; }

    public void Mapping(Profile p)
    {
        p.CreateMap<SpeciesSearchHistory, SpeciesSearchHistoryDto>().ReverseMap();
        p.CreateMap<SearchSpeciesQuery, SpeciesSearchHistoryDto>().ReverseMap();
    }
}
