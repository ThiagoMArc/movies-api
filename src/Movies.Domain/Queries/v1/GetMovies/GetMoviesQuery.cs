using Movies.Domain.Queries.Contracts;

namespace Movies.Domain.Queries.v1.GetMovies;
public class GetMoviesQuery : IQuery
{
    public GetMoviesQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
}
