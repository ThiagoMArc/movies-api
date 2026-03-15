using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Commands.v1.CreateMovie;

public class CreateMovieCommand : ICommand
{
    public CreateMovieCommand(string title, int releaseYear, string director,
                string synopsis, Dictionary<string, string> cast)
    {
        Title = title;
        ReleaseYear = releaseYear;
        Director = director;
        Synopsis = synopsis;
        Cast = cast;
    }
    public string Title { get; private set; }
    public int ReleaseYear { get; private set; }
    public string Director { get; private set; }
    public string Synopsis { get; private set; }
    public Dictionary<string, string> Cast { get; private set; }
}
