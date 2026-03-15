using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Movies.Domain.Entities;
public class Movie
{
    public Movie(string title, int releaseYear, string director, 
                string synopsis, Dictionary<string, string> cast)
    {
        Title = title;
        ReleaseYear = releaseYear;
        Director = director;
        Synopsis = synopsis;
        Cast = cast;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = default!;

    [BsonElement("title")]
    public string Title { get; private set; }

    [BsonElement("releaseYear")]
    public int ReleaseYear { get; private set; }

    [BsonElement("director")]
    public string Director { get; private set; }

    [BsonElement("synopsis")]
    public string Synopsis {get; private set;}

    [BsonElement("cast")]
    public Dictionary<string, string> Cast { get; private set; }

    public void UpdateMovieInfos(string title, string director, 
                                 string synopsis, int? releaseYear,
                                 Dictionary<string, string>? cast)
    {
        if(!string.IsNullOrWhiteSpace(title))
            Title = title;

        if(!string.IsNullOrWhiteSpace(director))
            Director = director;

        if(!string.IsNullOrWhiteSpace(synopsis))
            Synopsis = synopsis;

        if(releaseYear is not null)
            ReleaseYear = releaseYear.Value;

        if(cast is not null)
            Cast = cast;
    }
}
