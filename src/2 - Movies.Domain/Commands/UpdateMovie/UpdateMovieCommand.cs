using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Commands;
public class UpdateMovieCommand : ICommand
{
    public UpdateMovieCommand(string? id, string? title, 
                              int? releaseYear, string? director,
                              string? synopsis, Dictionary<string, string>? cast)
    {
        Id = id ?? " ";
        Title = title;
        ReleaseYear = releaseYear;
        Director = director;
        Synopsis = synopsis;
        Cast = cast;
    }

    public string Id { get; private set; }
    public string? Title { get; private set; }
    public int? ReleaseYear { get; private set; }
    public string? Director { get; private set; }
    public string? Synopsis { get; private set; }
    public Dictionary<string, string>? Cast { get; private set; }
}
