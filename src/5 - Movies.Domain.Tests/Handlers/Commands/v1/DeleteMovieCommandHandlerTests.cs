using Moq;
using Movies.Domain.Commands.v1.DeleteMovie;
using Movies.Domain.Entities;
using Movies.Domain.Handlers.Commands.v1.DeleteMovie;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;
using Shouldly;

namespace Movies.Domain.Tests.Handlers.Commands.v1;
public class DeleteMovieCommandHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepository = new(); 
    private readonly Mock<ICache<Movie>> _cacheMock = new();

    [InlineData("BFE129D91EA3E7BFA35BC6D1")]
    [InlineData("XPTO")]
    [Theory(DisplayName = "DeleteMovieCommandHandler should not be able to dele to delete non existent movie")]
    public async Task DeleteMovieCommandHandler_Should_Not_Be_Able_To_Delete_Non_Existent_Movie(string id)
    {
        //Arrange
        DeleteMovieCommand command = new(id);
        _movieRepository.Setup(m => m.GetById(id).Result).Returns((Movie)null);

        //Act
        GenericCommandResult result = await new DeleteMovieCommandHandler(_movieRepository.Object, _cacheMock.Object).Handle(command, CancellationToken.None);

        //Assert
        result.Success.ShouldBeFalse();
        result.Status.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "DeleteMovieCommandHandler should be able to delete a registered movie")]
    public async Task DeleteMovieCommandHandler_Should_Be_Able_To_Delete_A_Registered_Movie()
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
        _cacheMock.Setup(s => s.RemoveAsync(It.IsAny<string>(), CancellationToken.None)).Returns(Task.CompletedTask);

        //Act
        GenericCommandResult result = await new DeleteMovieCommandHandler(_movieRepository.Object, _cacheMock.Object).Handle(command, CancellationToken.None);

        //Assert
        result.Success.ShouldBeTrue();
        result.Status.ShouldBe(System.Net.HttpStatusCode.NoContent);
    }
}
