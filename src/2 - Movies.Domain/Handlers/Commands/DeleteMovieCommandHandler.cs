using MediatR;
using Movies.Domain.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Utils;

namespace Movies.Domain.Handlers.Commands;

public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, GenericCommandResult>
{
    private readonly IMovieRepository _movieRepository;

    public DeleteMovieCommandHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task<GenericCommandResult> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        if(!request.IsValid)
        {
            return new GenericCommandResult(false, "", StringFormat.ToString(request.Notifications.Select(m => m.Message).ToList()));
        }

        Movie? movie = await _movieRepository.GetById(request.Id);

        if(movie is null)
        {
            return new GenericCommandResult(false, "Can't delete non existent movie");
        }

        await _movieRepository.Delete(request.Id);

        return new GenericCommandResult(true, "Movie successfully deleted");
    }
}
