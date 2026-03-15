using FluentValidation;
using Movies.Domain.CrossCutting.Shared;
using Movies.Domain.Queries.v1.GetMovies;

namespace Movies.Domain.Queries.GetMovies.v1.GetMovies;
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