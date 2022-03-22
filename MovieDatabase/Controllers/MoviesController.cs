namespace MovieDatabase.Controllers
{
    using Movie;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ModelMovies = MovieDatabase.Models;
    using MovieDatabase.Exceptions;

    [Route("[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        }

        [HttpGet]
        public async Task<ActionResult> SearchMovieItems(
            [FromQuery] ModelMovies.MovieSearchInfo searchInfo,
            CancellationToken token)
        {
            try
            {
                var searchResult = await this.movieRepository.SearchAsync(searchInfo, token).ConfigureAwait(false);
                var movieList = new ModelMovies.MovieList { movies = searchResult };
                return this.Ok(movieList);
            }
            catch (ValidationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult> GetAsync(string id, CancellationToken token)
        {
            try
            {
                var movie = await this.movieRepository.GetAsync(id, token).ConfigureAwait(false);
                return this.Ok(movie);
            }
            catch (MovieNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpGet("{title}")]
        public async Task<ActionResult> GetByTitleAsync(string title, CancellationToken token)
        {
            try
            {
                var movieItems = await this.movieRepository.GetByTitleAsync(title, token).ConfigureAwait(false);
                var movieList = new ModelMovies.MovieList { movies = movieItems };
                return this.Ok(movieList);
            }
            catch (TitleNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieAsync(
            [FromBody] ModelMovies.MovieCreateInfo addInfo,
            CancellationToken token)
        {
            try
            {
                var movieInfo = await this.movieRepository.CreateAsync(addInfo, token).ConfigureAwait(false);
                return this.Ok(movieInfo);
            }
            catch (ValidationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(
            string id,
            [FromBody] ModelMovies.MovieUpdateInfo patchInfo,
            CancellationToken token)
        {
            try
            {
                await this.movieRepository.UpdateAsync(id, patchInfo, token).ConfigureAwait(false);
                return this.Ok();
            }
            catch (MovieNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken token)
        {
            try
            {
                await this.movieRepository.DeleteAsync(id, token).ConfigureAwait(false);
                return this.Ok();
            }
            catch (MovieNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}
