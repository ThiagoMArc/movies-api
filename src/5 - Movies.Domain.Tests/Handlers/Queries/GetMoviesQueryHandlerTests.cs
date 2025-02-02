using Shouldly;
using Moq;
using Movies.Domain.Entities;
using Movies.Domain.Handlers.Queries;
using Movies.Domain.Queries;
using Movies.Domain.Repositories;
using Movies.Domain.Results;

namespace Movies.Domain.Tests.Handlers.Queries;
public class GetMoviesQueryHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepository = new();

    [Fact(DisplayName = "GetMoviesQueryHandler returns movies with a valid request")]
    public async Task GetMoviesQueryHandler_Should_Be_Able_To_Return_Movies_With_Valid_Request()
    {
        //Arrange
        GetMoviesQuery request = new(0, 5);
        
        Dictionary<string, string> cast = new()
        {
            {"Micheal Keaton", "Batman"},
            {"Jack Nicholson", "Joker"}
        };

        IEnumerable<Movie> movies = new List<Movie>
        {
            new("Batman", 1989, "Tim Burton", "Face joker", cast)
        };

        _movieRepository.Setup(m => m.GetAll().Result).Returns(movies);

        //Act
        GenericQueryResult result = await new GetMoviesQueryHandler(_movieRepository.Object).Handle(request, CancellationToken.None);

        //Assert
        result.Success.ShouldBeTrue();
        result.Status.ShouldBe(System.Net.HttpStatusCode.OK);
    }

    [Theory(DisplayName = "GetMoviesQueryHandler can not return movies with invalid request")]
    [InlineData(-1,5)]
    [InlineData(0,0)]
    public async Task GetMoviesQueryHandler_Should_Not_Be_Able_To_Return_Movies_With_Invalid_Request(int pageIndex, int pageSize)
    {
        //Arrange
        GetMoviesQuery request = new(pageIndex, pageSize);

        //Act
        GenericQueryResult result = await new GetMoviesQueryHandler(_movieRepository.Object).Handle(request, CancellationToken.None);

        //Assert
        result.Success.ShouldBeFalse();
        result.Status.ShouldBe(System.Net.HttpStatusCode.BadRequest);
    }
}
