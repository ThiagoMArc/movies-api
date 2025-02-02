using Movies.Domain.Queries.Contracts;

namespace Movies.Domain.Queries;
public class GetMovieByIdQuery : IQuery
{
    public GetMovieByIdQuery(string? id)
    {
        Id = id ?? " ";
    }
    public string Id { get; set; }
}
