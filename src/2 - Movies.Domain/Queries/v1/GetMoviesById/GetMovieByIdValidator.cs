using FluentValidation;
using Movies.Domain.CrossCutting.Shared;

namespace Movies.Domain.Queries.v1.GetMoviesById;

public class GetMovieByIdValidator : AbstractValidator<GetMovieByIdQuery>
{
    public GetMovieByIdValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage(Constants.ID_MUST_BE_PROVIDED);
    }
}
