
using FluentValidation;

namespace Movies.Domain.Commands.CreateMovie;
public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Movie title must be provided");

        RuleFor(x => x.Director)
            .NotEmpty()
            .WithMessage("Movie director must be provided");

        RuleFor(x => x.Synopsis)
            .NotEmpty()
            .WithMessage("Movie synopsis must be provided");

        RuleFor(x => x.Cast)
            .NotEmpty()
            .WithMessage("Movie cast must be provided");

        RuleFor(x => x.ReleaseYear)
            .GreaterThan(0)
            .WithMessage("Movie release year must be greater than 0");
    }
}
