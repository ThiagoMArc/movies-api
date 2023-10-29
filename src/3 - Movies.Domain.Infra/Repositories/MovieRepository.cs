using MongoDB.Driver;
using Movies.Domain.Entities;
using Movies.Domain.Infra.Context;
using Movies.Domain.Repositories;

namespace Movies.Domain.Infra.Repositories;

public class MovieRepository : IMovieRepository
{
    private AppDbContext _context;

    public MovieRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Create(Movie movie)
    {
        await _context.Movies.InsertOneAsync(movie);
    }

    public async Task Delete(string id)
    {
        await _context.Movies.DeleteOneAsync(id);
    }

    public async Task<IEnumerable<Movie>> GetAll()
    {
        return await _context.Movies.Find(_ => true).ToListAsync();
    }

    public async Task<Movie> GetById(string id)
    {
        return await _context.Movies.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task Update(string id, Movie movie)
    {
        await _context.Movies.ReplaceOneAsync(m => m.Id == id, movie);
    }
}
