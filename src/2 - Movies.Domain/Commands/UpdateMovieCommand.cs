using Flunt.Notifications;
using Flunt.Validations;
using MongoDB.Bson;
using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Commands;
public class UpdateMovieCommand : Notifiable<Notification>, ICommand
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

    public void Validate()
    {
        AddNotifications(
            new Contract<Notification>()
                .Requires()
                .IsNotNullOrWhiteSpace(Id, "Id", "Id must be provided in order to update a movie")
        );

        if(!ObjectId.TryParse(Id, out _))
            AddNotification(nameof(Id), "Id must be a valid 24 digit hex string");
    }
}
