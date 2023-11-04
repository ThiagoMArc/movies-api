using Movies.Domain.Commands;

namespace Movies.Domain.Tests.Commands;
public class CreateMovieCommandTests
{
    [Fact(DisplayName = "It should be able to register movie with valid command")]
    public void Should_Be_Able_To_Register_A_Movie_With_Valid_Command()
    {
        //Arrange
        Dictionary<string, string> cast = new()
        {
            { "Tobey Maguire", "Spider Man" },
            { "Kirsten Durnst", "Mary Jame" },
            { "James Franco", "Harry Osborn" },
            { "Alfred Molina", "Dock Ock" }
        };

        CreateMovieCommand command = new("Spider Man 2", 2004, "Sam Raime", "Spidey is back in a new adventure to face dock ock", cast);

        //Act
        command.Validate();

        //Assert
        Assert.True(command.IsValid);
    }

    [Theory(DisplayName = "It should not be able to register movie with invalid command")]
    [MemberData(nameof(GetCreateInvalidConfigs))]
    public void Should_Not_Be_Able_To_Register_A_Movie_With_Invalid_Command(string movieTitle, 
                                                                            string director, 
                                                                            string synopsis, 
                                                                            Dictionary<string, 
                                                                            string> cast, 
                                                                            int releaseYear)
    {
        //Arrange
        CreateMovieCommand command = new(movieTitle, releaseYear, director, synopsis, cast);

        //Act
        command.Validate();

        //Assert
        Assert.False(command.IsValid);
    }

    public static IEnumerable<object[]> GetCreateInvalidConfigs()
    {
        var configs = new List<object[]>
            {
                new object[] { " ", "director", "movie synopsis", new Dictionary<string,string>(), 2004},
                new object[] { "movie", " ", "movie synopsis", new Dictionary<string,string>(), 2004 },
                new object[] { "movie", "director", " ", new Dictionary<string,string>(), 2004 },
                new object[] { "movie", "director", " ", new Dictionary<string,string>(), 2004 },
                new object[] { "movie", "director", "synopsis", null, 2004 },
                new object[] { "movie", "director", "synopsis", new Dictionary<string,string>(), 0 }
            };

        return configs;
    }
}
