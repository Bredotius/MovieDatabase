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
    internal sealed class DeleteMovieTests
    {
        private IMovieRepository movieRepository;

        [SetUp]
        public void SetUp()
        {
            this.movieRepository = new MovieRepository(new DatabaseConnection().Settings);
        }

        [Test]
        public void ThrowMovieNotFoundException_WhenGetDeletedMovie()
        {
            var movie = this.movieRepository.CreateAsync(new MovieCreateInfo(), default).Result;

            this.movieRepository.DeleteAsync(movie.Id.ToString(), default).Wait();

            Func<Task> action = async () =>
                await this.movieRepository.GetAsync(movie.Id.ToString(), default);
            action.Should().ThrowAsync<MovieNotFoundException>();
        }

        [Test]
        public void ThrowMovieNotFoundException_WhenMovieNotFound()
        {
            Func<Task> action = async () =>
                await this.movieRepository.DeleteAsync(Guid.NewGuid().ToString(), default);

            action.Should().ThrowAsync<MovieNotFoundException>();
        }
    }
}
