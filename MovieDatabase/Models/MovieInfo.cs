namespace MovieDatabase.Models
{
    using System;

    /// <summary>
    /// Информация о фильме
    /// </summary>
    public class MovieInfo
    {
        /// <summary>
        /// Идентификатор фильма
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название фильма
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Сюжет
        /// </summary>
        public string Storyline { get; set; }

        /// <summary>
        /// Жанр
        /// </summary>
        public string[] Genre { get; set; }

        /// <summary>
        /// Год выпуска фильма
        /// </summary>
        public int RealeaseYear { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public double Rating { get; set; }
    }
}
