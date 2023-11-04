using Movies.Domain.Queries;

namespace Movies.Domain.Tests.Queries;
public class GetMoviesQueryTests
{
    [Fact]
    public void Should_GetMovies_When_Query_Is_Valid()
    {
        //Arrange
        int pageIndex = 0;
        int pageSize = 5;
        GetMoviesQuery query = new(pageIndex, pageSize);

        //Act 
        query.Validate();

        //Assert
        Assert.True(query.IsValid);
    }

    [Theory]
    [InlineData(-1,5)]
    [InlineData(1,0)]
    [InlineData(-1,0)]
    public void Should_Not_Be_Able_To_GetMovies_When_Query_Is_Invalid(int pageIndex, int pageSize)
    {
        //Arrange
        GetMoviesQuery query = new(pageIndex, pageSize);

        //Act
        query.Validate();

        //Assert
        Assert.False(query.IsValid);
    }
}
