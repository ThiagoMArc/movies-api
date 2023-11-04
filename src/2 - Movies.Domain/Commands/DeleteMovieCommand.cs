using Flunt.Notifications;
using Flunt.Validations;
using MongoDB.Bson;
using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Commands;
public class DeleteMovieCommand : Notifiable<Notification>, ICommand
{
    public DeleteMovieCommand(string? id)
    {
        Id = id ?? " ";
    }

    public string Id { get; private set; }

    public void Validate()
    {
        AddNotifications(
            new Contract<Notification>()
                .Requires()
                .IsNotNullOrWhiteSpace(Id, "Id", "Id must be provided in order to delete a movie")
        );

        if(!ObjectId.TryParse(Id, out _))
            AddNotification(nameof(Id), "Id must be a valid 24 digit hex string");
        
    }
}
