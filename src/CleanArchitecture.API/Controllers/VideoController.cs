using CleanArchitecture.Application.Features.Videos.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class VideoController : ControllerBase
{
    private readonly IMediator _mediator;

    public VideoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{username}", Name = nameof(GetVideosByUsername))]
    [ProducesResponseType(typeof(IEnumerable<VideoDto>), (int)HttpStatusCode.OK)]
    [Authorize]
    public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideosByUsername(string username)
    {
        var query = new Application.Features.Videos.List.Query(username);
        var videos = await _mediator.Send(query);
        return Ok(videos);
    }

}
