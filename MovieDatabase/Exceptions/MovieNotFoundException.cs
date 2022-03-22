namespace MovieDatabase.Exceptions
{
    using System;

    /// <summary>
    /// Исключение, которое возникает при попытке получить несуществующий фильм
    /// </summary>
    public class MovieNotFoundException : Exception
    {
        /// <summary>
        /// Создает новый экземпляр исключения о том, что фильм не найден
        /// </summary>
        /// <param name="id">Идентификатор запрашиваемого фильма</param>
        public MovieNotFoundException(string id)
            : base($"Movie with id {id} is not found.")
        {
        }
    }

    /// <summary>
    /// Исключение, которое возникает при попытке получить фильм с несуществующим названием
    /// </summary>
    public class TitleNotFoundException : Exception
    {
        /// <summary>
        /// Создает новый экземпляр исключения о том, что фильм не найден
        /// </summary>
        /// <param name="title">Название запрашиваемого фильма</param>
        public TitleNotFoundException(string title)
            : base($"Movie with title {title} is not found.")
        {
        }
    }
}
