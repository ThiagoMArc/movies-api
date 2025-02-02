using Moq;
using Movies.Domain.Commands;
using Movies.Domain.Entities;
using Movies.Domain.Handlers.Commands;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Shouldly;

namespace Movies.Domain.Tests.Handlers.Commands;
public class CreateMovieCommandHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepository = new(); 

    [Fact(DisplayName = "CreateMovieCommandHandler should not be able to register an already registered movie")]
    public async Task CreateMovieCommandHandler_Should_Not_Be_Able_To_Register_An_Already_Regitered_Movie()
    {
        //Arrange
        Dictionary<string, string> cast = new()
        {
            {"Jamie Lee Curtis", "Laurie Strode"},
            {"Nick Castle", "Micheal Myers"}
        };

        CreateMovieCommand command = new("Halloween", 1978, "John Carpenter", "Synopsis", cast);
        Movie movie = new("Halloween", 1978, "John Carpenter", "Synopsis", cast);

        _movieRepository.Setup(m => m.GetByTitle("Halloween").Result).Returns(movie);

        //Act
        GenericCommandResult result = await new CreateMovieCommandHandler(_movieRepository.Object).Handle(command, CancellationToken.None);

        //Assert
        result.Success.ShouldBeFalse();
        result.Status.ShouldBe(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "CreateMovieCommandHandler should be able to register a movie")]
    public async Task CreateMovieCommandHandler_Should_Be_Able_To_Register_A_Movie()
    {
        //Arrange
        Dictionary<string, string> cast = new()
        {
            {"Arnold Schwarzernegger", "Harry Tasker"},
            {"Jamie Lee Curtis", "Helen Tasker"},
        };

        CreateMovieCommand command = new("True Lies", 1994, "James Cameron", "Synopsis", cast);
        
        Movie movie = new("True Lies", 1994, "James Cameron", "Synopsis", cast);

        _movieRepository.Setup(m => m.Create(movie)).Returns(Task.CompletedTask);

        //Act
        GenericCommandResult result = await new CreateMovieCommandHandler(_movieRepository.Object).Handle(command, CancellationToken.None);

        //Assert
        result.Success.ShouldBeTrue();
        result.Status.ShouldBe(System.Net.HttpStatusCode.OK);
        result.Data.ShouldNotBeNull();
    }
}
