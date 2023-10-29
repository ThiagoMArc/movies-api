using Movies.Domain.Queries.Contracts;

namespace Movies.Domain.Results;
public class GenericQueryResult : GenericResult, IQueryResult
{
    public GenericQueryResult(bool success, string? message = null, object? data = null) : base(success, message, data)
    {
    }
}