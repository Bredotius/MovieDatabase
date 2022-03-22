namespace MovieDatabase.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Параметры поиска задач
    /// </summary>
    public class MovieSearchInfo
    {
        /// <summary>
        /// Максимальное количество фильмов, которое нужно вернуть
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? Limit { get; set; } = 10;

        /// <summary>
        /// Название фильма
        /// </summary>
        [StringLength(125)]
        public string? Title { get; set; }

        /// <summary>
        /// Жанр
        /// </summary>
        [StringLength(25)]
        public string? Genre { get; set; }

        /// <summary>
        /// Минимальный год выпуска фильма
        /// </summary>
        [Range(1985, 2030)]
        public int? RealeaseFrom { get; set; }

        /// <summary>
        /// Максимальный год выпуска фильма
        /// </summary>
        [Range(1985, 2030)]
        public int? RealeaseTo { get; set; } = DateTime.UtcNow.Year;

        /// <summary>
        /// Минимальный рейтинг фильма
        /// </summary>
        [Range(1, 10)]
        public double? RatingFrom { get; set; }

        /// <summary>
        /// Максимальный рейтинг фильма
        /// </summary>
        [Range(1, 10)]
        public double? RatingTo { get; set; }

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortType? Sort { get; set; }

        /// <summary>
        /// Аспект фильма, по которому нужно искать
        /// </summary>
        public MovieSortBy? SortBy { get; set; }
    }
}
