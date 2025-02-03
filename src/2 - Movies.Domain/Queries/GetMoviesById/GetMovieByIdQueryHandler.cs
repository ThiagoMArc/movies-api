using MediatR;
using Movies.Domain.CrossCutting.Configuration;
using Movies.Domain.Entities;
using Movies.Domain.Queries;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;


namespace Movies.Domain.Handlers.Queries;
public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, GenericQueryResult>
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICache<Movie> _cache;

    public GetMovieByIdQueryHandler(
    IMovieRepository movieRepository,
    ICache<Movie> cache)
    {
        _movieRepository = movieRepository;
        _cache = cache;
    }
    public async Task<GenericQueryResult> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        bool movieInCache = _cache.TryGetValue(request.Id, out var movie);

        if (movieInCache)
            return new GenericQueryResult(success: true,
                                          data: new
                                          {
                                              id = request?.Id,
                                              title = movie?.Title,
                                              synopsis = movie?.Synopsis,
                                              releaseYear = movie?.ReleaseYear,
                                              director = movie?.Director,
                                              cast = movie?.Cast
                                          });

        movie = await _movieRepository.GetById(request.Id);

        if (movie is null)
            return new GenericQueryResult(success: false,
                                          status: System.Net.HttpStatusCode.NotFound);

        await _cache.SetAsync(request.Id, movie, CacheSettings.Configs);

        return new GenericQueryResult(success: true,
                                      data: new
                                      {
                                          id = movie?.Id,
                                          title = movie?.Title,
                                          synopsis = movie?.Synopsis,
                                          releaseYear = movie?.ReleaseYear,
                                          director = movie?.Director,
                                          cast = movie?.Cast
                                      });
    }
}
