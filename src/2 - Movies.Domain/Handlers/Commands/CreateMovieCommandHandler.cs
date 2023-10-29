using MediatR;
using Movies.Domain.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Utils;

namespace Movies.Domain.Handlers.Commands;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, GenericCommandResult>
{
    private readonly IMovieRepository _movieRepository;

    public CreateMovieCommandHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<GenericCommandResult> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        if(!request.IsValid)
        {
            return new GenericCommandResult(false, "", StringFormat.ToString(request.Notifications.Select(m => m.Message).ToList()));
        }

        if(await MovieAlreadyExists(request.Title))
        {
            return new GenericCommandResult(false, "Movie with given title is already registered", request.Title);
        }

        Movie movie = new (request.Title, request.ReleaseYear, 
                          request.Director, request.Synopsis, request.Cast);

        await _movieRepository.Create(movie);

        return new GenericCommandResult(false, "Movie registed with success", movie?.Id);
    }

    private async Task<bool> MovieAlreadyExists(string title)
    {
        return await _movieRepository.GetByTitle(title) != null;
    }
}
