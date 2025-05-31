using FluentValidation;
using Movies.Domain.CrossCutting.Shared;

namespace Movies.Domain.Queries.GetMovies;
public class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
{
    public GetMoviesQueryValidator()
    {
        RuleFor(x => x.PageIndex)
        .GreaterThan(0)
        .WithMessage(Constants.PAGE_MINIMUM_INDEX);   

        RuleFor(x => x.PageSize)
        .GreaterThan(0)
        .WithMessage(Constants.PAGE_MINIMUM_SIZE);
    }
}