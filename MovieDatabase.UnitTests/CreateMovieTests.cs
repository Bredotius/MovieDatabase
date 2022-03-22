using MovieDatabase.Movie;
using FluentAssertions;
using NUnit.Framework;
using MovieDatabase.Database;
using MovieDatabase.Models;

namespace MovieDatabase.UnitTests
{
    [TestFixture]
    internal sealed class CreateMovieTests
    {
        private IMovieRepository movieRepository;

        [SetUp]
        public void SetUp()
        {
            this.movieRepository = new MovieRepository(new DatabaseConnection().Settings);
        }

        [Test]
        public void GotCorrectMovie_WhenCreate()
        {
            var createInfo = new MovieCreateInfo
            {
                Title = "Прибытие поезда на вокзал города Ла-Сьота",
                Storyline = "Всемирно известная короткометражка про прибытие поезда на вокзал.",
                RealeaseYear = 1895,
                Genre = new[] { "документальный", "короткометражка" },
                Rating = 8.1
            };

            var movie = this.movieRepository.CreateAsync(createInfo, default).Result;

            movie.Title.Should().Be(createInfo.Title);
            movie.Storyline.Should().Be(createInfo.Storyline);
            movie.RealeaseYear.Should().Be(createInfo.RealeaseYear);
            movie.Rating.Should().Be(createInfo.Rating);
            movie.Genre.Should().BeEquivalentTo(createInfo.Genre);

        }
    }
}
