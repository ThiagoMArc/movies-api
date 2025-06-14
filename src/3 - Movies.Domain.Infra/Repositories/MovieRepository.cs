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
        await _context.Movies.DeleteOneAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Movie>> GetPagedData(int pageNumber, int pageSize)
    {
        return await _context.Movies.Find(_ => true)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Limit(pageSize)
                                    .ToListAsync();
    }

    public async Task<Movie> GetById(string id)
    {
        return await _context.Movies.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Movie> GetByTitle(string title)
    {
        return await _context.Movies.Find(m => m.Title == title).FirstOrDefaultAsync();
    }

    public async Task Update(Movie movie)
    {
        await _context.Movies.ReplaceOneAsync(m => m.Id == movie.Id, movie);
    }
}
