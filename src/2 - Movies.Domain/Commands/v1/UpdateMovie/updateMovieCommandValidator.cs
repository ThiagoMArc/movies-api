using FluentValidation;
using Movies.Domain.Commands.v1.UpdateMovie;

namespace Movies.Domain.Commands.UpdateMovie.v1.UpdateMovie;
public class updateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public updateMovieCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id must be provided in order to update a movie");
    }
}
