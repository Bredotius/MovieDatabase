namespace MovieDatabase.Movie
{
    using MongoDB.Driver;
    using MovieDatabase.Database;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MovieDatabase.Models;
    using Microsoft.Extensions.Options;
    using MovieDatabase.Exceptions;

    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<MovieInfo> movies;

        public MovieRepository(IOptions<MovieDatabaseSettings> movieDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                movieDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                movieDatabaseSettings.Value.DatabaseName);

            movies = mongoDatabase.GetCollection<MovieInfo>(
                movieDatabaseSettings.Value.MoviesCollectionName);
        }

        public MovieRepository(MovieDatabaseSettings movieDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                movieDatabaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                movieDatabaseSettings.DatabaseName);

            mongoDatabase.DropCollection(
                movieDatabaseSettings.MoviesCollectionName);

            movies = mongoDatabase.GetCollection<MovieInfo>(
                movieDatabaseSettings.MoviesCollectionName);
        }

        public async Task<List<MovieInfo>> SearchAsync(MovieSearchInfo searchInfo, CancellationToken token)
        {
            if (searchInfo == null)
            {
                throw new ArgumentNullException(nameof(searchInfo));
            }

            var builder = Builders<MovieInfo>.Filter;

            var filter = builder.Lte(m => m.RealeaseYear, searchInfo.RealeaseTo.Value);

            if (searchInfo.Title != null)
                filter &= builder.Eq(m => m.Title, searchInfo.Title);

            if (searchInfo.Genre != null)
                filter &= builder.AnyEq(m => m.Genre, searchInfo.Genre);

            if (searchInfo.RealeaseFrom != null)
                filter &= builder.Gte(m => m.RealeaseYear, searchInfo.RealeaseFrom.Value);

            if (searchInfo.RatingFrom != null)
                filter &= builder.Gte(m => m.Rating, searchInfo.RatingFrom.Value);

            if (searchInfo.RatingTo != null)
                filter &= builder.Lte(m => m.Rating, searchInfo.RatingTo.Value);

            var sort = searchInfo.Sort ?? SortType.Ascending;
            var sortBy = searchInfo.SortBy ?? MovieSortBy.Release;

            var searchResult = movies.Find(filter);

            if (sort == SortType.Ascending)
                searchResult = sortBy == MovieSortBy.Rating
                    ? searchResult.SortBy(m => m.Rating)
                    : searchResult.SortBy(m => m.RealeaseYear);
            else
                searchResult = sortBy == MovieSortBy.Rating
                    ? searchResult.SortByDescending(m => m.Rating)
                    : searchResult.SortByDescending(m => m.RealeaseYear);

            return await searchResult.Limit(searchInfo.Limit).ToListAsync(token);
            
        }

        public async Task<MovieInfo> GetAsync(string id, CancellationToken token)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                throw new MovieNotFoundException(id);
            }

            var result = await movies.FindAsync(m => m.Id == guidId, cancellationToken: token);

            if (result == null)
            {
                throw new MovieNotFoundException(id);
            }

            var movie = await result.FirstOrDefaultAsync();

            return movie;
        }

        public async Task<List<MovieInfo>> GetByTitleAsync(string title, CancellationToken token)
        {
            var result = movies.Find(m => m.Title == title);

            if (result == null)
            {
                throw new TitleNotFoundException(title);
            }

            return await result.ToListAsync(token);
        }

        public async Task<MovieInfo> CreateAsync(MovieCreateInfo movieInfo, CancellationToken token)
        {
            if (movieInfo == null)
            {
                throw new ArgumentNullException(nameof(movieInfo));
            }

            var movie = new MovieInfo
            {
                Id = Guid.NewGuid(),
                Title = movieInfo.Title,
                Storyline = movieInfo.Storyline,
                RealeaseYear = movieInfo.RealeaseYear,
                Genre = movieInfo.Genre,
                Rating = movieInfo.Rating,
            };

            await movies.InsertOneAsync(movie, cancellationToken: token);

            return movie;
        }

        public async Task UpdateAsync(string id, MovieUpdateInfo updateInfo, CancellationToken token)
        {
            if (updateInfo is null)
                throw new ArgumentNullException(nameof(updateInfo));

            var movie = await GetAsync(id, token);

            if (movie is null)
                throw new MovieNotFoundException(id);

            var patch = updateInfo.Title is null ?
                Builders<MovieInfo>.Update.Set(m => m.Title, movie.Title) :
                Builders<MovieInfo>.Update.Set(m => m.Title, updateInfo.Title);

            if (updateInfo.Storyline != null)
                patch = patch.Set(m => m.Storyline, updateInfo.Storyline);

            if (updateInfo.RealeaseYear != null)
                patch = patch.Set(m => m.RealeaseYear, updateInfo.RealeaseYear);

            if (updateInfo.Rating != null)
                patch = patch.Set(m => m.Rating, updateInfo.Rating);

            if (updateInfo.Genre != null)
                patch = patch.Set(m => m.Genre, updateInfo.Genre);

            await movies.UpdateOneAsync(m => m.Id == movie.Id, patch, cancellationToken: token);
        }

        public async Task DeleteAsync(string id, CancellationToken token)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                throw new MovieNotFoundException(id);
            }

            var movie = await GetAsync(id, token);

            var deleteResult = await movies.DeleteOneAsync(m => m.Id == guidId, cancellationToken: token);
        }
    }
}
