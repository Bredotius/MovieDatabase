namespace MovieDatabase.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Информация для добавления фильма
    /// </summary>
    [DataContract]
    public class MovieCreateInfo
    {
        /// <summary>
        /// Название фильма
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(125, MinimumLength = 3)]
        public string Title { get; set; }

        /// <summary>
        /// Сюжет
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(300, MinimumLength = 3)]
        public string Storyline { get; set; }

        /// <summary>
        /// Жанр
        /// </summary>
        [DataMember(IsRequired = true)]
        [MaxLength(5)]
        public string[] Genre { get; set; }

        /// <summary>
        /// Год выпуска фильма
        /// </summary>
        [DataMember(IsRequired = true)]
        [Range(1895, 2030)]
        public int RealeaseYear { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        [DataMember(IsRequired = true)]
        [Range(1, 10)]
        public double Rating { get; set; }
    }
}
