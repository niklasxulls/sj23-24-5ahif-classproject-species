using DiveSpecies.Application.Models;
using DiveSpecies.Application.UseCases.SightingNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Remove;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Update;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiveSpecies.Rest.Controllers;

[Route("api/sighting")]
public class SightingController : ApiControllerBase
{

    [HttpPost, Route("")]
    public async Task<ActionResult<string>> AddSighting([FromBody] AddSightingCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpPut, Route("")]
    public async Task<IActionResult> UpdateSighting([FromBody] UpdateSightingCommand cmd)
    {
        await Mediator.Send(cmd);

        return Ok();
    }

    [HttpDelete, Route("")]
    public async Task<IActionResult> RemoveSighting([FromQuery] RemoveSightingCommand cmd)
    {
        await Mediator.Send(cmd);
    
        return Ok();
    }
}

