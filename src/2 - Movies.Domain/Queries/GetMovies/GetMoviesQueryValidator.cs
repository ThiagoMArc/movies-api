using FluentValidation;

namespace Movies.Domain.Queries.GetMovies;
public class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
{
    public GetMoviesQueryValidator()
    {
        RuleFor(x => x.PageIndex)
        .GreaterThan(0)
        .WithMessage("Page index must be at least 1");   

        RuleFor(x => x.PageSize)
        .GreaterThan(0)
        .WithMessage("Page size must have a minimum size of 1");
    }
}