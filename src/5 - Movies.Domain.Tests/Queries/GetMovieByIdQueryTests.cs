using Movies.Domain.Queries;
using Shouldly;

namespace Movies.Domain.Tests.Queries;
public class GetMovieByIdQueryTests
{
    [Fact(DisplayName = "Validate returns true with valid query")]
    public void Should_Be_Able_To_Get_A_Movie_When_Valid_Id_Is_Provided()
    {
        //Arrange
        string id = "6015F447A2C5EF4B2EED169F";
        GetMovieByIdQuery query = new(id);

        //Act
        query.Validate();

        //Assert
        query.IsValid.ShouldBeTrue();
    }

    [Theory(DisplayName = "Validate returns false with invalid query")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1234")]
    public void Should_Not_Be_Able_To_Get_A_Movie_When_Query_Is_Invalid(string id)
    {
        //Arrange
        GetMovieByIdQuery query = new(id);

        //Act
        query.Validate();

        //Assert
        query.IsValid.ShouldBeFalse();
    }
}
