using FluentValidation;
using MongoDB.Bson;

namespace Movies.Domain.Commands.UpdateMovie;
public class updateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public updateMovieCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id must be provided in order to update a movie")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Id must be a valid 24 digit hex string");
    }
}
