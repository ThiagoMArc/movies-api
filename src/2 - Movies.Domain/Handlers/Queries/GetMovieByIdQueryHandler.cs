using MediatR;
using Movies.Domain.Entities;
using Movies.Domain.Queries;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Utils;


namespace Movies.Domain.Handlers.Queries;
public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, GenericQueryResult>
{
    private readonly IMovieRepository _movieRepository;

    public GetMovieByIdQueryHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    public async Task<GenericQueryResult> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        request.Validate();

        if(!request.IsValid)
        {
            return new GenericQueryResult(false, "", StringFormat.ToString(request.Notifications.Select(m => m.Message).ToList()));
        }

        Movie? movie = await _movieRepository.GetById(request.Id);

        if(movie is null)
        {
            return new GenericQueryResult(false, "There is no such movie with provided id", request.Id);
        }

        return new GenericQueryResult(true, "Movie successfully found", movie);
    }
}
