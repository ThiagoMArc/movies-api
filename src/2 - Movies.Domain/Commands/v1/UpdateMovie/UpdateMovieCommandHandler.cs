using MediatR;
using Movies.Domain.Shared.Helpers;
using Movies.Domain.Commands.v1.UpdateMovie;
using Movies.Domain.Entities;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;

namespace Movies.Domain.Handlers.Commands.v1.UpdateMovie;

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, GenericCommandResult>
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICache<Movie> _cache;

    public UpdateMovieCommandHandler(
        IMovieRepository movieRepository,
        ICache<Movie> cache)
    {
        _movieRepository = movieRepository;
        _cache = cache;
    }

    public async Task<GenericCommandResult> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        if (!DomainHelper.MovieExists(request.Id))
            return new GenericCommandResult(success: false,
                                            status: System.Net.HttpStatusCode.NotFound,
                                            data: $"Movie with id: {request?.Id} not found");

        Movie ? movie = await _movieRepository.GetById(request.Id);

        if (movie is null)
            return new GenericCommandResult(success: false,
                                            status: System.Net.HttpStatusCode.NotFound,
                                            data: $"Movie with id: {request?.Id} not found");

        movie.UpdateMovieInfos(request?.Title ?? string.Empty,
                               request?.Director ?? string.Empty,
                               request?.Synopsis ?? string.Empty,
                               request?.ReleaseYear,
                               request?.Cast);

        await _movieRepository.Update(movie);

        await _cache.RemoveAsync(movie.Id);

        return new GenericCommandResult(success: true,
                                        data: movie);
    }
}
