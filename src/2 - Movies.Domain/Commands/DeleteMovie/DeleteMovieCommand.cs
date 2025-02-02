using Movies.Domain.Commands.Contracts;

namespace Movies.Domain.Commands;
public class DeleteMovieCommand : ICommand
{
    public DeleteMovieCommand(string? id)
    {
        Id = id ?? string.Empty;
    }

    public string Id { get; private set; }
}
