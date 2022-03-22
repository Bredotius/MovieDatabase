namespace MovieDatabase.Services
{
    using Microsoft.Extensions.DependencyInjection;
    using MovieDatabase.Movie;

    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMovies(this IServiceCollection services)
        {
            services.AddSingleton<IMovieRepository, MovieRepository>();
            return services;
        }
    }
}
