using MovieDatabase.Database;

namespace MovieDatabase.UnitTests
{
    internal class DatabaseConnection
    {
        public MovieDatabaseSettings Settings = new MovieDatabaseSettings()
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "MovieRepository",
            MoviesCollectionName = "Movies"
        };
    }
}
