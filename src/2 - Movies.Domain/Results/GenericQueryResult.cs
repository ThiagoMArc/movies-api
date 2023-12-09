using System.Net;
using Movies.Domain.Queries.Contracts;

namespace Movies.Domain.Results;
public class GenericQueryResult : GenericResult, IQueryResult
{
    public GenericQueryResult(bool success, HttpStatusCode status = HttpStatusCode.OK, object? data = null) : base(success, status, data)
    {
    }
}