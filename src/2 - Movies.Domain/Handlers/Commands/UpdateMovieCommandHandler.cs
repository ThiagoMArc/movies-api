using MediatR;
using Movies.Domain.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;
using Movies.Domain.Utils;

namespace Movies.Domain.Handlers.Commands;

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, GenericCommandResult>
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICache<Movie> _cache;

    public UpdateMovieCommandHandler(
        IMovieRepository movieRepository, 
        ICache<Movie>  cache)
    {
        _movieRepository = movieRepository;
        _cache = cache;
    }

    public async Task<GenericCommandResult> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        if(!request.IsValid)
            return new GenericCommandResult(success: false, 
                                            status: System.Net.HttpStatusCode.BadRequest, 
                                            data: StringFormat.ToString(request.Notifications.Select(m => m.Message).ToList()));

        Movie? movie = await _movieRepository.GetById(request.Id);

        if(movie is null)
            return new GenericCommandResult(success: false, 
                                            status: System.Net.HttpStatusCode.NotFound);

        movie.UpdateMovieInfos(request?.Title, 
                               request?.Director, 
                               request?.Synopsis, 
                               request?.ReleaseYear,
                               request?.Cast);

        await _movieRepository.Update(request.Id, movie);

        await _cache.RemoveAsync(request.Id);

        return new GenericCommandResult(success: true, 
                                        data: movie);
    }
}
