using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Results;

public class GenericCommandResult : GenericResult, ICommandResult
{
    public GenericCommandResult(bool success, string? message = null, object? data = null): base(success, message, data)
    {
    }   
}
