namespace MovieDatabase.Models
{
    /// <summary>
    /// Аспект задачи для сортировки
    /// </summary>
    public enum MovieSortBy
    {
        /// <summary>
        /// Сортировкаи по дате создания
        /// </summary>
        Release = 0,

        /// <summary>
        /// Сортировка по рейтингу
        /// </summary>
        Rating,
    }
}
