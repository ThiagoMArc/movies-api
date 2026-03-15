using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Commands.v1.UpdateMovie;
public class UpdateMovieCommand : ICommand
{
    public UpdateMovieCommand(string id, string title, 
                              int? releaseYear, string director,
                              string synopsis, Dictionary<string, string>? cast)
    {
        Id = id ?? " ";
        Title = title;
        ReleaseYear = releaseYear;
        Director = director;
        Synopsis = synopsis;
        Cast = cast;
    }

    public string Id { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public int? ReleaseYear { get; private set; }
    public string Director { get; private set; } = default!;
    public string Synopsis { get; private set; } = default!;
    public Dictionary<string, string>? Cast { get; private set; }
}
