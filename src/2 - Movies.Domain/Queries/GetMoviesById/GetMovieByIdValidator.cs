using MongoDB.Bson;
using FluentValidation;

namespace Movies.Domain.Queries.GetMoviesById;

public class GetMovieByIdValidator : AbstractValidator<GetMovieByIdQuery>
{
    public GetMovieByIdValidator()
    {
        RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("Id must be provided in order to find movie")
        .Must(id => ObjectId.TryParse(id, out _))
        .WithMessage("Id must be a valid 24 digit hex string");
    }
}
