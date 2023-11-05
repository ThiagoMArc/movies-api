using Moq;
using Movies.Domain.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Handlers.Commands;
using Movies.Domain.Repositories;
using Movies.Domain.Results;

namespace Movies.Domain.Tests.Handlers.Commands;
public class DeleteMovieCommandHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepository = new(); 

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("11231345")]
    public async Task DeleteMovieCommandHandler_Should_Not_Be_Able_To_Delete_Movie_With_Invalid_Request(string id)
    {
        //Arrange
        DeleteMovieCommand command = new(id);

        //Act
        GenericCommandResult result = await new DeleteMovieCommandHandler(_movieRepository.Object).Handle(command, CancellationToken.None);

        //Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task DeleteMovieCommandHandler_Should_Not_Be_Able_To_Delete_Non_Existent_Movie()
    {
        //Arrange
        string id = "1E195FA36F9B5BD41814FE43";
        DeleteMovieCommand command = new(id);
        _movieRepository.Setup(m => m.GetById(id).Result).Returns((Movie)null);

        //Act
        GenericCommandResult result = await new DeleteMovieCommandHandler(_movieRepository.Object).Handle(command, CancellationToken.None);

        //Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task DeleteMovieCommandHandler_Should_Be_Able_To_Delete_An_Existent_Movie()
    {
        //Arrange
        string id = "653ec34b4b6e63d9c724cbf2";

        DeleteMovieCommand command = new(id);
        
        Dictionary<string, string> cast = new()
        {
            {"Micheal Keaton", "Batman"},
            {"Jack Nicholson", "Joker"}
        };
        
        Movie movie = new("Batman", 1989, "Tim Burton", "Faces joker", cast);
        
        _movieRepository.Setup(m => m.GetById(id).Result).Returns(movie);
        _movieRepository.Setup(m => m.Delete(id)).Returns(Task.CompletedTask);

        //Act
        GenericCommandResult result = await new DeleteMovieCommandHandler(_movieRepository.Object).Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.Success);
    }

}