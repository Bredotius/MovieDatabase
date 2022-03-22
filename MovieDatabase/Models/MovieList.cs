namespace MovieDatabase.Models
{
    using System.Collections.Generic;

    /// <summary>
    ///  Список c описанием фильмов
    /// </summary>
    public sealed class MovieList
    {
        /// <summary>
        /// Информация о фильмах
        /// </summary>
        public IReadOnlyCollection<MovieInfo> movies { get; set; }
    }
}
