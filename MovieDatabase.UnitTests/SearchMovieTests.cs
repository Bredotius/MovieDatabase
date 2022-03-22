using System;
using System.Threading;
using FluentAssertions;
using MovieDatabase.Models;
using MovieDatabase.Movie;
using NUnit.Framework;

namespace MovieDatabase.UnitTests
{
    [TestFixture]
    internal sealed class SearchMovieTests
    {
        private IMovieRepository movieRepository;

        [SetUp]
        public void SetUp()
        {
            this.movieRepository = new MovieRepository(new DatabaseConnection().Settings);
        }

        [Test]
        public void GotMoviesRealeasedAfterYear_WhenRealeaseFromNotEmpty()
        {
            var movie1 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { RealeaseYear = 1999}, default).Result;
            var movie2 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { RealeaseYear = 2005 }, default).Result;
            var movie3 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { RealeaseYear = 2011 }, default).Result;

            var moviesList = this.movieRepository
                .SearchAsync(new MovieSearchInfo { RealeaseFrom = 2003 }, default).Result;

            moviesList.Should().BeEquivalentTo(new[] { movie2, movie3 });
        }

        [Test]
        public void GotMoviesRealeasedAfterBeforeYear_WhenRealeaseToNotEmpty()
        {
            var movie1 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { RealeaseYear = 1999 }, default).Result;
            var movie2 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { RealeaseYear = 2005 }, default).Result;
            var movie3 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { RealeaseYear = 2011 }, default).Result;

            var moviesList = this.movieRepository
                .SearchAsync(new MovieSearchInfo { RealeaseTo = 2009 }, default).Result;

            moviesList.Should().BeEquivalentTo(new[] { movie1, movie2 });
        }

        [Test]
        public void GotMoviesWithGenre_WhenGenreNotEmpty()
        {
            var movie1 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { Genre = new[] { "фантастика", "боевик" } }, default).Result;
            var movie2 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { Genre = new[] { "боевик", "драма" } }, default).Result;
            var movie3 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { Genre = new[] { "триллер", "криминал" } }, default).Result;

            var moviesList = this.movieRepository
                .SearchAsync(new MovieSearchInfo { Genre = "боевик" }, default).Result;

            moviesList.Should().BeEquivalentTo(new[] { movie1, movie2 });
        }

        [Test]
        public void GotMoviesWithLimit_WhenGenreWithLimitNotEmpty()
        {
            var movie1 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { Genre = new[] { "фантастика" } }, default).Result;
            var movie2 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { Genre = new[] { "боевик", "драма" } }, default).Result;
            var movie3 = this.movieRepository
                .CreateAsync(new MovieCreateInfo { Genre = new[] { "фантастика" } }, default).Result;

            var moviesList = this.movieRepository
                .SearchAsync(new MovieSearchInfo { Genre = "фантастика", Limit = 1 }, default).Result;

            moviesList.Should().BeEquivalentTo(new[] { movie1 });
        }

        [Test]
        public void GotTenMovies_WhenLimitIsEmpty()
        {
            for (var i = 0; i < 15; i++)
            {
                this.movieRepository.CreateAsync(new MovieCreateInfo(), default).Wait();
            }

            var postsList = this.movieRepository.SearchAsync(new MovieSearchInfo(), CancellationToken.None).Result;

            postsList.Should().HaveCount(10);
        }
    }
}
