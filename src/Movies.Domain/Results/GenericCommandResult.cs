using System.Net;
using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Results;

public class GenericCommandResult : GenericResult, ICommandResult
{
    public GenericCommandResult(bool success, HttpStatusCode status = HttpStatusCode.OK, object? data = null): base(success, status, data)
    {
    }   
}
