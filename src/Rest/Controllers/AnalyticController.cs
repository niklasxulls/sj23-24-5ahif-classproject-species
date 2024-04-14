using DiveSpecies.Application.Models;
using DiveSpecies.Application.Models.DTOs.SpeciesNS;
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using DiveSpecies.Application.UseCases.SightingNS.Queries.SpeciesPerLocation;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Remove;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Update;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiveSpecies.Rest.Controllers;

[Route("api/analytic")]
public class AnalyticController : ApiControllerBase
{
    [HttpGet, Route("location")]
    public async Task<ActionResult<ICollection<SpeciesPerLocationCluster>>> GetSpeciesPerLocation([FromQuery] GetSpeciesPerLocationQuery query)
    {
        return (await Mediator.Send(query)).ToList();
    }

    [HttpGet, Route("depth")]
    public async Task<ActionResult<ICollection<GetSpeciesDepthAnalysisGroupDto>>> GetSpeciesDepthAnalysis([FromQuery] GetSpeciesDepthAnalyticsQuery query)
    {
        return (await Mediator.Send(query)).ToList();
    }
}
