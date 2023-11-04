
using Moq;
using Movies.Domain.Commands;

namespace Movies.Domain.Tests.Commands;
public class UpdateMovieCommandTests
{

    [Fact]
    public void Should_Be_Able_To_Update_Movie_Infos_With_Valid_Command()
    {
        //Arrange
        UpdateMovieCommand command = new("6015F447A2C5EF4B2EED169F", It.IsAny<string>(), 
                                        It.IsAny<int>(), It.IsAny<string>(), 
                                        It.IsAny<string>(), 
                                        It.IsAny<Dictionary<string, string>>());

        //Act
        command.Validate();

        //Assert
        Assert.True(command.IsValid);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("12323434")]
    public void Should_Not_Be_Able_To_Update_Movie_Info_With_Invalid_Command(string id)
    {
        //Arrange
        UpdateMovieCommand command = new(id, It.IsAny<string>(), 
                                        It.IsAny<int>(), It.IsAny<string>(), 
                                        It.IsAny<string>(), 
                                        It.IsAny<Dictionary<string, string>>());

        //Act
        command.Validate();

        //Assert
        Assert.False(command.IsValid);
    }
}
