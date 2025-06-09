using System.Net;
using Shouldly;
using Moq;
using Movies.Domain.Entities;
using Movies.Domain.Handlers.Queries.v1.GetMoviesById;
using Movies.Domain.Queries.v1.GetMoviesById;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;

namespace Movies.Domain.Tests.Handlers.Queries.v1;
public class GetMovieByIdQueryHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepository = new();
    private readonly Mock<ICache<Movie>> _cacheMock = new();

    [InlineData("wrfere566")]
    [InlineData("BFE129D91EA3E7BFA35BC6D1")]
    [Theory(DisplayName = "GetMovieByIdQueryHandler should not be able to get non existent movie")]
    public async Task GetMovieByIdQueryHandler_Should_Not_Be_Able_To_Get_Non_Existent_Movie(string id)
    {
        //Arrange
        GetMovieByIdQuery request = new(id);

        Movie? movie = null;

        _cacheMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out movie)).Returns(false);

        _movieRepository.Setup(m => m.GetById(id).Result).Returns((Movie)null);

        //Act
        GenericQueryResult result = await new GetMovieByIdQueryHandler(_movieRepository.Object, _cacheMock.Object).Handle(request, CancellationToken.None);

        //Assert
        result.Success.ShouldBeFalse();
        result.Status.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "GetMovieByIdQueryHandler should be able to return a movie from cache")]
    public async Task GetMovieByIdQueryHandler_Should_Be_Able_To_Return_A_Movie_From_Cache()
    {
        //Arrange
        string movieId = "56028E4CF3752AC81007F316";

        GetMovieByIdQuery request = new(movieId);

        Movie? movie = null;

        Dictionary<string, string> cast = new()
        {
            {"Al Pacino", "Micheal Corleone"},
            {"Marlon Brando", "Don Vito Corleone"}
        };

        movie = new("The Godfather", 1972, "Frances Ford Copolla", "Thrilling", cast);
        
        _cacheMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out movie)).Returns(true);

        //Act
        GenericQueryResult result = await new GetMovieByIdQueryHandler(_movieRepository.Object, _cacheMock.Object).Handle(request, CancellationToken.None);

        //Assert
        result.Success.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Data.ShouldNotBeNull();
    }

    [Fact(DisplayName = "GetMovieByIdQueryHandler should be able to return a movie from database")]
    public async Task GetMovieByIdQueryHandler_Should_Be_Able_To_Return_A_Movie_From_Database()
    {
        //Arrange
        string movieId = "46028E4CF3732AC81005F314";

        GetMovieByIdQuery request = new(movieId);

        Movie? movie = null;

        _cacheMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out movie)).Returns(false);

        Dictionary<string, string> cast = new()
        {
            {"Micheal Keaton", "Batman"},
            {"Jack Nicholson", "Joker"}
        };

        movie = new("Batman", 1989, "Tim Burton", "Faces joker", cast);

        _movieRepository.Setup(m => m.GetById(movieId).Result).Returns(movie);

        //Act
        GenericQueryResult result = await new GetMovieByIdQueryHandler(_movieRepository.Object, _cacheMock.Object).Handle(request, CancellationToken.None);

        //Assert
        result.Success.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Data.ShouldNotBeNull();
    }
}
