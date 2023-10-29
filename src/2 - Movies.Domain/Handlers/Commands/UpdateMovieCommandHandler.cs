using MediatR;
using Movies.Domain.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Utils;

namespace Movies.Domain.Handlers.Commands;

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, GenericCommandResult>
{
    private readonly IMovieRepository _movieRepository;

    public UpdateMovieCommandHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<GenericCommandResult> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        if(!request.IsValid)
        {
            return new GenericCommandResult(false, "", StringFormat.ToString(request.Notifications.Select(m => m.Message).ToList()));
        }

        Movie? movie = await _movieRepository.GetById(request.Id);

        if(movie is null)
        {
            return new GenericCommandResult(false, "Can't update non existent movie", request.Id);
        }

        movie.UpdateMovieInfos(request.Title, request.Director, 
                               request.Synopsis, request.ReleaseYear, request.Cast);

        await _movieRepository.Update(request.Id, movie);

        return new GenericCommandResult(true, "Movies infos updated successfully", movie);
    }
}
