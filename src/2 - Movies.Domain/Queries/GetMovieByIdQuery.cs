using Flunt.Notifications;
using Flunt.Validations;
using MongoDB.Bson;
using Movies.Domain.Queries.Contracts;

namespace Movies.Domain.Queries;
public class GetMovieByIdQuery : Notifiable<Notification>, IQuery
{
    public GetMovieByIdQuery(string? id)
    {
        Id = id ?? " ";
    }
    public string Id { get; set; }

    public void Validate()
    {
        AddNotifications(
            new Contract<Notification>()
                .Requires()
                .IsNotNullOrWhiteSpace(Id, "Id", "Id must be provided in order to find movie")
        );

        if(!ObjectId.TryParse(Id, out _))
        {
            AddNotification(nameof(Id), "Id must be a valid 24 digit hex string");
        }
    }
}
