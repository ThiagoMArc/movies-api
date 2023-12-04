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
        Assert.False(result.Success);
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
        Assert.False(result.Success);
    }

    [Fact]
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
        Assert.True(result.Success);
    }
}
