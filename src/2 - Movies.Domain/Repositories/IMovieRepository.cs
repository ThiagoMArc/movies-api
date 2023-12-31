using MongoDB.Bson;
using Movies.Domain.Entities;

namespace Movies.Domain.Repositories;

public interface IMovieRepository
{
    Task Create(Movie movie);
    Task Update(string id, Movie movie);
    Task Delete(string id);
    Task<Movie> GetById(string id);
    Task<Movie> GetByTitle(string title);
    Task<IEnumerable<Movie>> GetAll();
}
