using System.Net;
using Shouldly;
using Moq;
using Movies.Domain.Entities;
using Movies.Domain.Handlers.Queries;
using Movies.Domain.Queries;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;

namespace Movies.Domain.Tests.Handlers.Queries;
public class GetMovieByIdQueryHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepository = new();
    private readonly Mock<ICache<Movie>> _cacheMock = new();

    [Theory(DisplayName = "GetMovieByIdQueryHandler should not be able to get a movie with invalid query")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("3453453")]
    [InlineData("")]
    public async Task GetMovieByIdQueryHandler_Should_Not_Be_Able_To_Get_Movie_With_Invalid_Request(string id)
    {
        //Arrange
        GetMovieByIdQuery request = new(id);

        //Act
        GenericQueryResult result = await new GetMovieByIdQueryHandler(_movieRepository.Object, _cacheMock.Object).Handle(request, CancellationToken.None);

        //Assert
        result.Status.ShouldBe(HttpStatusCode.BadRequest);
        result.Success.ShouldBeFalse();
    }

    [Fact(DisplayName = "GetMovieByIdQueryHandler should not be able to get non existent movie")]
    public async Task GetMovieByIdQueryHandler_Should_Not_Be_Able_To_Get_Non_Existent_Movie()
    {
        //Arrange
        string nonExistentMovieId = "BFE129D91EA3E7BFA35BC6D1";

        GetMovieByIdQuery request = new(nonExistentMovieId);

        Movie? movie = null;

        _cacheMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out movie)).Returns(false);

        _movieRepository.Setup(m => m.GetById(nonExistentMovieId).Result).Returns((Movie)null);

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
