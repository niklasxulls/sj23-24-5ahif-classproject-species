using DiveSpecies.Application.Models;
using DiveSpecies.Application.Models.DTOs.SpeciesNS;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Remove;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Update;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetDetails;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetLatestSearch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiveSpecies.Rest.Controllers;

[Route("api/species")]
public class SpeciesController : ApiControllerBase
{

    [HttpPost, Route("")]
    public async Task<ActionResult<string>> AddSpecies([FromBody] AddSpeciesCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpPut, Route("")]
    public async Task<ActionResult<string>> UpdateSpecies([FromBody] UpdateSpeciesCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpDelete, Route("")]
    public async Task<IActionResult> RemoveSpecies([FromQuery] RemoveSpeciesCommand cmd)
    {
        await Mediator.Send(cmd);
    
        return Ok();
    }

    [HttpGet, Route("{speciesId}")]
    public async Task<ActionResult<SpeciesDetailsDto>> GetSpeciesDetails(string speciesId)
    {
        return await Mediator.Send(new GetSpeciesDetailsQuery{ SpeciesId = speciesId });
    }

    [HttpGet, Route("search")]
    public async Task<ActionResult<PagedResult<SpeciesShallowDto>>> SearchSpecies([FromQuery] SearchSpeciesQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet, Route("search/history")]
    public async Task<ActionResult<ICollection<SpeciesSearchHistoryDto>>> GetSearchHistory([FromQuery] GetSpeciesSearchHistoryQuery query)
    {
        return (await Mediator.Send(query)).ToList();
    }
}
