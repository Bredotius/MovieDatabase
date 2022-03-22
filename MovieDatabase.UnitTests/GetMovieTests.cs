using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MovieDatabase.Movie;
using MovieDatabase.Models;
using NUnit.Framework;
using MovieDatabase.Exceptions;

namespace MovieDatabase.UnitTests
{
    [TestFixture]
    internal sealed class GetMovieTests
    {
        private IMovieRepository firstMovieRepository;
        private IMovieRepository secondMovieRepository;

        [SetUp]
        public void SetUp()
        {
            this.firstMovieRepository = new MovieRepository(new DatabaseConnection().Settings);
            this.secondMovieRepository = new MovieRepository(new DatabaseConnection().Settings);
        }

        [Test]
        public void GotFromSecondRepository_WhenCreateInFirst()
        {
            var createInfo = new MovieCreateInfo
            {
                Title = "Прибытие поезда на вокзал города Ла-Сьота",
                Storyline = "Всемирно известная короткометражка про прибытие поезда на вокзал.",
                RealeaseYear = 1895,
                Genre = new[] { "документальный", "короткометражка" },
                Rating = 8.1
            };
            var expected = this.firstMovieRepository.CreateAsync(createInfo, default).Result;

            var actual = this.secondMovieRepository.GetAsync(expected.Id.ToString(), default).Result;

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GotByTitle_WhenTitleOccursOnce()
        {
            var createInfo = new MovieCreateInfo
            {
                Title = "Прибытие поезда на вокзал города Ла-Сьота",
                Storyline = "Всемирно известная короткометражка про прибытие поезда на вокзал.",
                RealeaseYear = 1895,
                Genre = new[] { "документальный", "короткометражка" },
                Rating = 8.1
            };
            var expected = this.firstMovieRepository.CreateAsync(createInfo, default).Result;

            var actual = this.secondMovieRepository.GetByTitleAsync(expected.Title, default).Result;

            actual[0].Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GotByTitle_WhenTitleOccursMultipleTimes()
        {
            var createInfo1 = new MovieCreateInfo
            {
                Title = "Бэтмен",
                Storyline = "Мститель в маске защищает Готэм от Джокера-весельчака.",
                RealeaseYear = 1989,
                Genre = new[] { "фантастика", "боевик", "триллер", "криминал" },
                Rating = 7.5
            };
            var createInfo2 = new MovieCreateInfo
            {
                Title = "Бэтмен",
                Storyline = "После двух лет поисков правосудия на улицах Готэма для своих сограждан Бэтмен становится олицетворением беспощадного возмездия.",
                RealeaseYear = 2022,
                Genre = new[] { "боевик", "драма", "криминал", "детектив" },
                Rating = 7.9
            };
            var expected1 = this.firstMovieRepository.CreateAsync(createInfo1, default).Result;
            var expected2 = this.firstMovieRepository.CreateAsync(createInfo2, default).Result;

            var actual = this.secondMovieRepository.GetByTitleAsync(expected1.Title, default).Result;

            actual.Should().BeEquivalentTo(new List<MovieInfo> { expected1, expected2 });
        }

        [Test]
        public void ThrowMovieNotFoundException_WhenMovieNotFound()
        {
            Func<Task> action = async () =>
                await this.firstMovieRepository.GetAsync(Guid.NewGuid().ToString(), default);

            action.Should().ThrowAsync<MovieNotFoundException>();
        }
    }
}
