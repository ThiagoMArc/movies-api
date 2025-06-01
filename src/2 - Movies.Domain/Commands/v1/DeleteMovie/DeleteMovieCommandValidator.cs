using FluentValidation;
using Movies.Domain.CrossCutting.Shared;

namespace Movies.Domain.Commands.v1.DeleteMovie;

public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(Constants.ID_MUST_BE_PROVIDED);
    }
}