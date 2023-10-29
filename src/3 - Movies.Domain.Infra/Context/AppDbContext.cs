using MongoDB.Driver;
using Movies.Domain.Entities;

namespace Movies.Domain.Infra.Context;
public class AppDbContext
{
    private readonly IMongoDatabase _database;

    public AppDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Movie> Movies => _database.GetCollection<Movie>("movies");
}
