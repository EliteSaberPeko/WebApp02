using System.ComponentModel.DataAnnotations;
using WebApp02.Interfaces;
namespace WebApp02.Models
{
    public class Genre : IModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно для заполнения")]
        [Display(Name = "Название")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "В названии допустимо от 3 до 20 символов")]
        public string? Name { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
