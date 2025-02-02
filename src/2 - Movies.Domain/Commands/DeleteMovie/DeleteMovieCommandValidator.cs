using MongoDB.Bson;
using FluentValidation;

namespace Movies.Domain.Commands.DeleteMovie;

public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id must be provided in order to delete a movie")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Id must be a valid 24 digit hex string");
    }
}