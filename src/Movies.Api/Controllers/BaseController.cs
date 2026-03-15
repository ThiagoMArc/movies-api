using Microsoft.AspNetCore.Mvc;
using Movies.Domain.Results;

namespace Movies.Domain.Api.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult GenerateResponse(GenericResult result)
    {
        return result.Status switch 
        {
            System.Net.HttpStatusCode.OK => Ok(result?.Data),
            System.Net.HttpStatusCode.Created => Created("", result?.Data),
            System.Net.HttpStatusCode.NoContent => NoContent(),
            System.Net.HttpStatusCode.BadRequest => BadRequest(result),
            System.Net.HttpStatusCode.NotFound => NotFound(result),
            _ => Problem(statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}
