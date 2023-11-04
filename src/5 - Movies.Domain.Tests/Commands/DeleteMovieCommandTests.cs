using Movies.Domain.Commands;

namespace Movies.Domain.Tests.Commands;
public class DeleteMovieCommandTests
{
    [Fact]
    public void Should_Be_Able_To_Delete_A_Movie_With_Valid_Command()
    {
        //Arrange
        DeleteMovieCommand command = new("6015F447A2C5EF4B2EED169F");

        //Act
        command.Validate();

        //Assert
        Assert.True(command.IsValid);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("123123255")]
    [InlineData(null)]
    public void Should_Not_Be_Able_To_Delete_A_Movie_With_Invalid_Command(string id)
    {
        //Arrange
        DeleteMovieCommand command = new(id);

        //Act
        command.Validate();

        //Assert
        Assert.False(command.IsValid);
    }
}
