using Microsoft.AspNetCore.Mvc;
using Movies.Domain.Results;
using System.Net;

namespace Movies.Domain.Api.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult GenerateResponse(GenericResult result)
    {
        return result.Status switch
        {
            HttpStatusCode.OK => Ok(result),
            HttpStatusCode.Created => Created("", result),
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.BadRequest => BadRequest(result),
            HttpStatusCode.NotFound => NotFound(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
