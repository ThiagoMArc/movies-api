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

        if (!request.IsValid)
            return new GenericCommandResult(success: false,
                                            status: System.Net.HttpStatusCode.BadRequest,
                                            data: StringFormat.ToString(request.Notifications.Select(m => m.Message).ToList()));

        if (await MovieAlreadyExists(request.Title))
            return new GenericCommandResult(success: false,
                                            status: System.Net.HttpStatusCode.BadRequest,
                                           data: $"Movie with given title {request?.Title} is already registered");


        Movie movie = new(request.Title, request.ReleaseYear,
                          request.Director, request.Synopsis, request.Cast);

        await _movieRepository.Create(movie);

        return new GenericCommandResult(success: true, 
                                        data: movie);
    }

    private async Task<bool> MovieAlreadyExists(string title)
    {
        return await _movieRepository.GetByTitle(title) != null;
    }
}
