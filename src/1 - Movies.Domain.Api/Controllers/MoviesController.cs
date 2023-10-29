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

    [HttpGet("{id}", Name = "Search a movie by id")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GenericQueryResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(string id)
    {
        GenericQueryResult result = await _mediator.Send(new GetMovieByIdQuery(id));

        if (!result.Success)
        {
            return BadRequest($"{(string.IsNullOrEmpty(result?.Message) ? result?.Data: result.Message + " " + result?.Data)}");
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
            return BadRequest($"{(string.IsNullOrEmpty(result?.Message) ? result?.Data: result.Message + " " + result?.Data)}");
        }

        return Ok(result?.Data);
    }

}
