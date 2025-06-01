using Shouldly;
using Moq;
using Movies.Domain.Commands.v1.UpdateMovie;
using Movies.Domain.Entities;
using Movies.Domain.Handlers.Commands.v1.UpdateMovie;
using Movies.Domain.Repositories;
using Movies.Domain.Results;
using Movies.Domain.Services;

namespace Movies.Domain.Tests.Handlers.Commands.v1;

public class UpdateMovieCommandHandlerTests
{
    private readonly Mock<IMovieRepository> _movieRepository = new();
    private readonly Mock<ICache<Movie>> _cacheMock = new();

    [Fact(DisplayName = "UpdateMovieCommandHandler should not be able to update non registered movie")]
    public async Task UpdateMovieCommandHandler_Should_Not_Be_Able_To_Update_Non_Registered_Movie()
    {
        //Arrange
        string id = "DB5E92267528BB9C59EF423B";

        UpdateMovieCommand command = new(id,
                                         "Terminator 78",
                                         2078,
                                         "James Cameron III",
                                         "He's back again",
                                         new Dictionary<string, string>() { { "Benson John", "T1908" } });

        _movieRepository.Setup(m => m.GetById(id).Result).Returns((Movie)null);

        //Act
        GenericCommandResult result = await new UpdateMovieCommandHandler(_movieRepository.Object, _cacheMock.Object).Handle(command, CancellationToken.None);

        //Assert
        result.Success.ShouldBeFalse();
        result.Status.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "UpdateMovieCommandHandler should be able to update movies infos")]
    public async Task UpdateMovieCommandHandler_Should_Be_Able_To_Update_Movie_Infos()
    {
        //Arrange
        string id = "D4C8D19458BEC5BAD9179047";

        Dictionary<string, string> cast = new()
        {
            {"Arnold", "T800"}
        };

        UpdateMovieCommand command = new(id,
                                         "Terminator 2",
                                         1991,
                                         "James Cameron",
                                         "He's back to save John and Sarah Connor",
                                         cast);

        _movieRepository.Setup(m => m.GetById(id).Result)
                        .Returns(new Movie(
                                        "Terminator 2",
                                            1992,
                                        "James Cameron",
                                        "He's back to save John and Sarah Connor",
                                        cast
                                        )
                                );

        _movieRepository.Setup(m => m.Update(new Movie(
                                                          "Terminator 2",
                                                           1991,
                                                          "James Cameron",
                                                          "He's back to save John and Sarah Connor",
                                                          cast)))
                        .Returns(Task.CompletedTask);

        _cacheMock.Setup(s => s.RemoveAsync(It.IsAny<string>(), CancellationToken.None)).Returns(Task.CompletedTask);

        //Act
        GenericCommandResult result = await new UpdateMovieCommandHandler(_movieRepository.Object, _cacheMock.Object).Handle(command, CancellationToken.None);

        //Assert
        result.Success.ShouldBeTrue();
        result.Status.ShouldBe(System.Net.HttpStatusCode.OK);
        result.Data.ShouldNotBeNull();
    }
}
