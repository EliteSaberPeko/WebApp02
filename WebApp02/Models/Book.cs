using System.ComponentModel.DataAnnotations;

namespace WebApp02.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно для заполнения")]
        [Display(Name = "Название")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "В названии допустимо от 1 до 100 символов")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        /*[Required(ErrorMessage = "Издательство обязательно для заполнения")]
        [Display(Name = "Издательство")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "В издательстве допустимо от 2 до 100 символов")]
        public string PublishingHouse { get; set; }*/
        //[Required(ErrorMessage = "Издательство обязательно для заполнения")]
        [Display(Name = "Издательство")]
        public PublishingHouse? PublishingHouse { get; set; }

        [Required(ErrorMessage = "Год публикации обязателен")]
        [Display(Name = "Год публикации")]
        [Range(typeof(uint), "0", "9999", ErrorMessage = "Год допустим от 0 до 9999")]
        public uint PublicationYear { get; set; }

        [Display(Name = "Количество экземпляров")]
        [Range(typeof(uint), "0", "9999999", ErrorMessage = "Количество экземпляров от 0 до 9999999")]
        public uint Count { get; set; }

        [Display(Name = "Цена, руб.")]
        [Range(typeof(decimal), "0,00", "9999999,99", ErrorMessage = "Цена должна быть больше, либо равна 0")]
        public decimal Price { get; set; }

        [Display(Name = "Список авторов")]
        public List<Autor> Autors { get; set; } = new List<Autor>();
        [Display(Name = "Список жанров")]
        public List<Genre> Genres { get; set; } = new List<Genre>();
    }
}
