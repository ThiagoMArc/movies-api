using Flunt.Notifications;
using Flunt.Validations;
using Movies.Domain.Queries.Contracts;

namespace Movies.Domain.Queries;
public class GetMoviesQuery : Notifiable<Notification>, IQuery
{
    public GetMoviesQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }

    public void Validate()
    {
        AddNotifications(
            new Contract<Notification>()
                .Requires()
                .IsGreaterOrEqualsThan(PageIndex, 0, "Page index can't be a negative number")
                .IsGreaterOrEqualsThan(PageSize, 1, "Page size must have a minimum size of 1")
        );
    }
}
