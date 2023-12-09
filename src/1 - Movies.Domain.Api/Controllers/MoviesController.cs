using MediatR;
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

    [HttpGet("{pageIndex}/{pageSize}", Name = "Search for movies and return paged results")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(GenericQueryResult))]
    public async Task<IActionResult> GetCharactersPaged(int pageIndex, int pageSize)
    {
        return GenerateResponse(await _mediator.Send(new GetMoviesQuery(pageIndex, pageSize)));
    }

    [HttpGet("{id}", Name = "Search a movie by id")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(GenericQueryResult))]
    public async Task<IActionResult> GetById(string id)
    {
        return GenerateResponse(await _mediator.Send(new GetMovieByIdQuery(id)));
    }

    [HttpPost(Name = "Creates a movie")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(GenericCommandResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Create([FromBody] CreateMovieCommand request)
    {
        return GenerateResponse(await _mediator.Send(request));
    }

    [HttpPut(Name = "Update movie infos")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericCommandResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Update([FromBody] UpdateMovieCommand request)
    {
        return GenerateResponse(await _mediator.Send(request));
    }

    [HttpDelete("{id}", Name = "Deletes a movie")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Delete(string id)
    {
        return GenerateResponse(await _mediator.Send(new DeleteMovieCommand(id)));
    }
}