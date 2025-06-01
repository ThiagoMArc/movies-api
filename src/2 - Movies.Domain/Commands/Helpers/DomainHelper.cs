using MongoDB.Bson;

namespace Movies.Domain.Commands.Helpers;

public static class DomainHelper
{
    public static bool MovieExists(string movieId)
    {
        return ObjectId.TryParse(movieId, out _);
    }
}
