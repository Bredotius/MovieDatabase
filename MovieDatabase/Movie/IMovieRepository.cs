namespace MovieDatabase.Movie
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MovieDatabase.Models;

    public interface IMovieRepository
    {
        /// <summary>
        /// Возвращает список фильмов соответствующих параметрам
        /// </summary>
        /// <param name="searchInfo">Праметры поиска</param>
        Task<List<MovieInfo>> SearchAsync(MovieSearchInfo searchInfo, CancellationToken token);

        /// <summary>
        /// Возвращает информацию о фильме по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор запрашиваемого фильма</param>
        Task<MovieInfo> GetAsync(string id, CancellationToken token);

        /// <summary>
        /// Возвращает информацию о фильмах по заданному названию
        /// </summary>
        /// <param name="title">Название запрашиваемого фильма</param>
        Task<List<MovieInfo>> GetByTitleAsync(string title, CancellationToken token);

        /// <summary>
        /// Создает запись о фильме по передаваемой информации
        /// </summary>
        /// <param name="movieInfo">Информация о фильме</param>
        Task<MovieInfo> CreateAsync(MovieCreateInfo movieInfo, CancellationToken token);

        /// <summary>
        /// Обновляет запись о фильме по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор обновляемого фильма</param>
        Task UpdateAsync(string id, MovieUpdateInfo updateInfo, CancellationToken token);

        /// <summary>
        /// Удаляет запись о фильме по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемого фильма</param>
        Task DeleteAsync(string id, CancellationToken token);
    }
}