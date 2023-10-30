using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Domain.Commands;
using Movies.Domain.Queries;
using Movies.Domain.Results;

namespace Movies.Domain.Api.Controllers;

[ApiController, ApiVersion("1"), Produces("application/json")]
[Route("api/v{version:ApiVersion}/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("paged/{pageIndex}/{pageSize}", Name = "Search for movies and return paged results")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericQueryResult))]
    public async Task<IActionResult> GetCharactersPaged(int pageIndex, int pageSize)
    {
        GenericQueryResult result = await _mediator.Send(new GetMoviesQuery(pageIndex, pageSize));
        
        if(!result.Success)
        {
            return BadRequest($"{(string.IsNullOrEmpty(result?.Message) ? result?.Data : result.Message + " " + result?.Data)}");
        }

        return Ok(result.Data);
    }

    [HttpGet("{id}", Name = "Search a movie by id")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(string id)
    {
        GenericQueryResult result = await _mediator.Send(new GetMovieByIdQuery(id));

        if (!result.Success)
        {
            return BadRequest($"{(string.IsNullOrEmpty(result?.Message) ? result?.Data : result.Message + " " + result?.Data)}");
        }

        return Ok(result?.Data);
    }

    [HttpPost(Name = "Creates a movie")]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(GenericCommandResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Create([FromBody] CreateMovieCommand request)
    {
        GenericCommandResult result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest($"{(string.IsNullOrEmpty(result?.Message) ? result?.Data : result.Message + " " + result?.Data)}");
        }

        return Created($"v1/characters", result?.Data);
    }

    [HttpPut(Name = "Update movie infos")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericCommandResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Update([FromBody] UpdateMovieCommand request)
    {
        GenericCommandResult result = await _mediator.Send(request);

        if (!result.Success)
        {
            return BadRequest($"{(string.IsNullOrEmpty(result?.Message) ? result?.Data : result.Message + " " + result?.Data)}");
        }

        return Ok(result?.Data);
    }

    [HttpDelete("{id}", Name = "Deletes a movie")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
    public async Task<IActionResult> Delete(string id)
    {
        GenericCommandResult result = await _mediator.Send(new DeleteMovieCommand(id));

        if (!result.Success)
        {
            return BadRequest($"{(string.IsNullOrEmpty(result?.Message) ? result?.Data : result.Message + " " + result?.Data)}");
        }

        return Ok(result?.Data);
    }

}
