
using Moq;
using Movies.Domain.Commands;
using Shouldly;

namespace Movies.Domain.Tests.Commands;
public class UpdateMovieCommandTests
{

    [Fact(DisplayName = "It should be able to update movie infos with valid command")]
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
        command.IsValid.ShouldBeTrue();
    }

    [Theory(DisplayName = "It should not be able to update movie infos with invalid command")]
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
        command.IsValid.ShouldBeFalse();
    }
}
