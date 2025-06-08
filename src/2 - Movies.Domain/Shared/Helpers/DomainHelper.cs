using MongoDB.Bson;

namespace Movies.Domain.Shared.Helpers;

public static class DomainHelper
{
    public static bool MovieExists(string movieId)
    {
        return ObjectId.TryParse(movieId, out _);
    }
}
