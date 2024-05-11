using CleanArchitecture.Application.Features.Directors.Dtos;
using CleanArchitecture.Identity.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DirectorController : ControllerBase
{
    private readonly IMediator _mediator;

    public DirectorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = nameof(CreateDirector))]
    [Authorize(Roles = ApplicationUserRoles.Admin)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CreateDirector([FromBody] CreateDirectorDto payload)
    {
        var command = new Application.Features.Directors.Create.Command(payload);
        return await _mediator.Send(command);
    }

}
