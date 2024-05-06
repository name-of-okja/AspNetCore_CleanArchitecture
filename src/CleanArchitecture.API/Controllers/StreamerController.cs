using CleanArchitecture.Application.Features.Streamers.Dtos;
using CleanArchitecture.Identity.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StreamerController : ControllerBase
{
    private readonly IMediator _mediator;

    public StreamerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = nameof(CreateStreamer))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [Authorize(Roles = ApplicationUserRoles.Admin)]
    public async Task<ActionResult<int>> CreateStreamer([FromBody]CreateStreamerDto payload)
    {
        var command = new Application.Features.Streamers.Create.Command(payload);

        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id}", Name = nameof(UpdateStreamer))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateStreamer(int id, [FromBody]EditStreamerDto payload)
    {
        var command = new Application.Features.Streamers.Edit.Command(id) { Payload = payload };

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}", Name = nameof(DeleteStreamer))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteStreamer(int id)
    {
        var command = new Application.Features.Streamers.Delete.Command(id);

        await _mediator.Send(command);

        return NoContent();
    }
}
