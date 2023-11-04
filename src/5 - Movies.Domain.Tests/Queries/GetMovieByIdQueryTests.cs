using Movies.Domain.Queries;

namespace Movies.Domain.Tests.Queries;
public class GetMovieByIdQueryTests
{
    [Fact]
    public void Should_Be_Able_To_Get_A_Movie_When_Valid_Id_Is_Provided()
    {
        //Arrange
        string id = "6015F447A2C5EF4B2EED169F";
        GetMovieByIdQuery movieQuery = new(id);

        //Act
        movieQuery.Validate();

        //Assert
        Assert.True(movieQuery.IsValid);
    }

    [Theory]
    [InlineData("",false)]
    [InlineData(null, false)]
    [InlineData("1234", false)]
    public void Should_Not_Be_Able_To_Get_A_Movie_When_Query_Is_Invalid(string id, bool expectedResult)
    {
        //Arrange
        GetMovieByIdQuery movieQuery = new(id);

        //Act
        movieQuery.Validate();

        //Assert
        Assert.Equal(movieQuery.IsValid, expectedResult);
    }
}
