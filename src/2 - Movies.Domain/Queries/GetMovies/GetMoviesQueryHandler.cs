using MediatR;
using Movies.Domain.Entities;
using Movies.Domain.Queries;
using Movies.Domain.Repositories;
using Movies.Domain.Results;

namespace Movies.Domain.Handlers.Queries;
public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, GenericQueryResult>
{
    private readonly IMovieRepository _movieRepository;

    public GetMoviesQueryHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<GenericQueryResult> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Movie> movies = await _movieRepository.GetPagedData(request.PageIndex, request.PageSize);

        return new GenericQueryResult(success: true,
                                      data: new
                                      {
                                          currentPage = request.PageIndex,
                                          pageSize = request.PageSize,
                                          result = movies
                                      });

    }
}
