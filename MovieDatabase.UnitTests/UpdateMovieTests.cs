using System;
using System.Threading.Tasks;
using FluentAssertions;
using MovieDatabase.Exceptions;
using MovieDatabase.Models;
using MovieDatabase.Movie;
using NUnit.Framework;

namespace MovieDatabase.UnitTests
{
    [TestFixture]
    internal sealed class UpdateMovieTests
    {
        private IMovieRepository movieRepository;

        [SetUp]
        public void SetUp()
        {
            this.movieRepository = new MovieRepository(new DatabaseConnection().Settings);
        }

        [Test]
        public void OneFieldUpdate_WhenOneFieldNotNull()
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
            var updateInfo = new MovieUpdateInfo 
            { 
                Storyline = "Один из первых фильмов, снятых и публично показанных братьями Люмьер.", 
                RealeaseYear = 1896 
            };

            this.movieRepository.UpdateAsync(movie.Id.ToString(), updateInfo, default).Wait();

            var updatedMovie = this.movieRepository.GetAsync(movie.Id.ToString(), default).Result;
            updatedMovie.Title.Should().Be(movie.Title);
            updatedMovie.Storyline.Should().Be(updateInfo.Storyline);
            updatedMovie.RealeaseYear.Should().Be(updateInfo.RealeaseYear);
            updatedMovie.Rating.Should().Be(movie.Rating);
            updatedMovie.Genre.Should().BeEquivalentTo(movie.Genre);
        }

        [Test]
        public void AllFieldsUpdate_WhenAllFieldsNotNull()
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
            var updateInfo = new MovieUpdateInfo
            {
                Title = "Прибытие поезда",
                Storyline = "Один из первых фильмов, снятых и публично показанных братьями Люмьер.",
                RealeaseYear = 1896,
                Genre = new[] { "документальный" },
                Rating = 7.4
            };

            this.movieRepository.UpdateAsync(movie.Id.ToString(), updateInfo, default).Wait();

            var updatedMovie = this.movieRepository.GetAsync(movie.Id.ToString(), default).Result;
            updatedMovie.Title.Should().Be(updateInfo.Title);
            updatedMovie.Storyline.Should().Be(updateInfo.Storyline);
            updatedMovie.RealeaseYear.Should().Be(updateInfo.RealeaseYear);
            updatedMovie.Rating.Should().Be(updateInfo.Rating);
            updatedMovie.Genre.Should().BeEquivalentTo(updateInfo.Genre);
        }

        [Test]
        public void ThrowMovieNotFoundException_WhenMovieNotFound()
        {
            Func<Task> action = async () =>
                await this.movieRepository.UpdateAsync(Guid.NewGuid().ToString(), new MovieUpdateInfo(), default);

            action.Should().ThrowAsync<MovieNotFoundException>();
        }
    }
}
