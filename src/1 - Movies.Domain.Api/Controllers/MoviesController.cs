using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Domain.Commands;
using Movies.Domain.Queries;
using Movies.Domain.Results;

namespace Movies.Domain.Api.Controllers;

[ApiController, ApiVersion("1"), Produces("application/json")]
[Route("api/v{version:ApiVersion}/movie")]
public class MoviesController : BaseController
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{pageIndex}/{pageSize}", Name = "Get Movies")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(GenericQueryResult))]
    public async Task<IActionResult> GetCharactersPaged(int pageIndex, int pageSize)
    {
        return GenerateResponse(await _mediator.Send(new GetMoviesQuery(pageIndex, pageSize)));
    }

    [HttpGet("{id}", Name = "Get Movie")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(GenericQueryResult))]
    public async Task<IActionResult> GetById(string id)
    {
        return GenerateResponse(await _mediator.Send(new GetMovieByIdQuery(id)));
    }

    [Authorize]
    [HttpPost(Name = "Register Movie")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(GenericCommandResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Create([FromBody] CreateMovieCommand request)
    {
        return GenerateResponse(await _mediator.Send(request));
    }

    [Authorize]
    [HttpPut(Name = "Update Movie")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericCommandResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Update([FromBody] UpdateMovieCommand request)
    {
        return GenerateResponse(await _mediator.Send(request));
    }

    [Authorize]
    [HttpDelete("{id}", Name = "Delete Movie")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Delete(string id)
    {
        return GenerateResponse(await _mediator.Send(new DeleteMovieCommand(id)));
    }
}