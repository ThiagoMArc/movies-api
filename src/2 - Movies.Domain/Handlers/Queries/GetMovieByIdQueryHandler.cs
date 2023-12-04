using MediatR;
using Movies.Domain.CrossCutting.Configuration;
using Movies.Domain.Entities;
using Movies.Domain.Queries;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;
using Movies.Domain.Utils;


namespace Movies.Domain.Handlers.Queries;
public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, GenericQueryResult>
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICache<Movie> _cache;

    public GetMovieByIdQueryHandler(
    IMovieRepository movieRepository, 
    ICache<Movie>  cache)
    {
        _movieRepository = movieRepository;
        _cache = cache;
    }
    public async Task<GenericQueryResult> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        request.Validate();

        if(!request.IsValid)
            return new GenericQueryResult(false, "", StringFormat.ToString(request.Notifications.Select(m => m.Message).ToList()));
        
        bool movieInCache = _cache.TryGetValue(request.Id, out var movie); 

        if(movieInCache)
            return new GenericQueryResult(true, "Movie successfully found", movie);    

        movie = await _movieRepository.GetById(request.Id);

        if(movie is null)
            return new GenericQueryResult(false, "There is no such movie with provided id", request.Id);

        await _cache.SetAsync(request.Id, movie, CacheSettings.Configs);

        return new GenericQueryResult(true, "Movie successfully found", movie);
    }
}
