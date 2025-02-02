using MediatR;
using Movies.Domain.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;

namespace Movies.Domain.Handlers.Commands;

public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, GenericCommandResult>
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICache<Movie> _cache;

    public DeleteMovieCommandHandler(
        IMovieRepository movieRepository, 
        ICache<Movie> cache)
    {
        _movieRepository = movieRepository;
        _cache = cache;
    }
    
    public async Task<GenericCommandResult> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        Movie? movie = await _movieRepository.GetById(request.Id);

        if(movie is null)
            return new GenericCommandResult(success: false,
                                            status: System.Net.HttpStatusCode.NotFound);

        await _movieRepository.Delete(request.Id);

        await _cache.RemoveAsync(request.Id);

        return new GenericCommandResult(success:true,
                                        status: System.Net.HttpStatusCode.NoContent);
    }
}
