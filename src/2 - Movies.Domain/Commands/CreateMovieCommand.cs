using Flunt.Notifications;
using Flunt.Validations;
using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Commands;

public class CreateMovieCommand : Notifiable<Notification>, ICommand
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

    public void Validate()
    {
        AddNotifications(
            new Contract<Notification>()
                .Requires()
                .IsNotNullOrWhiteSpace(Title, "Title", "Movie title must be provided")
                .IsNotNullOrWhiteSpace(Director, "Director", "Movie director must be provided")
                .IsNotNullOrWhiteSpace(Synopsis, "Synopsis", "Movie synopsis must be provided")
                .IsNotNull(Cast, "Cast", "Movie cast must be provided")
                .IsGreaterThan(ReleaseYear, 0, "ReleaseYear", "Movie release year must be greater than 0")
        );

        if (!Cast.Any())
        {
            AddNotification(nameof(Cast), "Movie cast must be provided");
        }
    }
}
