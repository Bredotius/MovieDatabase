namespace MovieDatabase.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Информация для изменения задачи
    /// </summary>
    [DataContract]
    public class MovieUpdateInfo
    {
        /// <summary>
        /// Название фильма
        /// </summary>
        [DataMember]
        [StringLength(125, MinimumLength = 3)]
        public string Title { get; set; }

        /// <summary>
        /// Сюжет
        /// </summary>
        [DataMember]
        [StringLength(300, MinimumLength = 3)]
        public string Storyline { get; set; }

        /// <summary>
        /// Жанр
        /// </summary>
        [DataMember]
        [MaxLength(5)]
        public string[] Genre { get; set; }

        /// <summary>
        /// Год выпуска фильма
        /// </summary>
        [DataMember]
        [Range(1895, 2030)]
        public int? RealeaseYear { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        [DataMember]
        [Range(1895, 2030)]
        public double? Rating { get; set; }
    }
}
