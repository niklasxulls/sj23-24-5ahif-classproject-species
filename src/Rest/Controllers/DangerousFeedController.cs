using DiveSpecies.Application.Models;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Queries.GetDetails;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Add;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Remove;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Commands.Update;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Queries;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Queries.GetByDate;
using DiveSpecies.Application.UseCases.DangerousFeedNS.Queries.GetDetails;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Add;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Remove;
using DiveSpecies.Application.UseCases.SpeciesNS.Commands.Update;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries;
using DiveSpecies.Application.UseCases.SpeciesNS.Queries.GetDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiveSpecies.Rest.Controllers;

[Route("api/dangerous-feed")]
public class DangerousFeedController : ApiControllerBase
{

    [HttpPost, Route("")]
    public async Task<ActionResult<string>> AddDangerousFeed([FromBody] AddDangerousFeedCommand cmd)
    {
        return await Mediator.Send(cmd);
    }

    [HttpPut, Route("")]
    public async Task<IActionResult> UpdateDangerousFeed([FromBody] UpdateDangerousFeedCommand cmd)
    {
        await Mediator.Send(cmd);

        return Ok();
    }

    [HttpDelete, Route("")]
    public async Task<IActionResult> RemoveDangerousFeed([FromQuery] RemoveDangerousFeedCommand cmd)
    {
        await Mediator.Send(cmd);
    
        return Ok();
    }

    [HttpGet, Route("{dangerousFeedId}")]
    public async Task<ActionResult<DangerousFeedDetailsDto>> GetDangerousFeedDetails(string dangerousFeedId)
    {
        return await Mediator.Send(new GetDangerousFeedDetailsQuery { DangerousFeedId = dangerousFeedId });
    }

    [HttpGet, Route("search")]
    public async Task<ActionResult<PagedResult<DangerousFeedShallowDto>>> SearchDangerousFeeds([FromQuery] SearchDangerousFeedQuery query)
    {
        return await Mediator.Send(query);
    }
}
