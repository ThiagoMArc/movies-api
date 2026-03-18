using System.Text.Json;
using ValidationException = Movies.Domain.CrossCutting.Exceptions.ValidationException;
using BadRequestException = Movies.Domain.CrossCutting.Exceptions.BadRequestException;
using Movies.Domain.Results;

namespace Movies.Domain.Api.Middlewares;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new GenericResult(success: false,
                                         status: (System.Net.HttpStatusCode)statusCode,
                                         data: null,
                                         messages: GetErrors(exception));

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };

    private static IList<string> GetErrors(Exception exception)
    {
        IList<string> errors = [];

        if (exception is ValidationException validationException)
        {
            errors = [.. validationException.ErrorsDictionary.SelectMany(kvp => kvp.Value)];
        }

        return errors;
    }
}
