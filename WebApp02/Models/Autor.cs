using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp02.Interfaces;

namespace WebApp02.Models
{
    public class Autor : IModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно для заполнения")]
        [Display(Name = "Имя")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "В имени допустимо от 2 до 30 символов")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
        [Display(Name = "Фамилия")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "В фамилии допустимо от 2 до 30 символов")]
        public string? LastName { get; set; }

        [Display(Name = "Отчество")]
        [MaxLength(30, ErrorMessage = "В отчестве допустимо до 30 символов")]
        public string? Patronymic { get; set; }
        [Required(ErrorMessage = "Дата рождения обязательна для заполнения")]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();

        [NotMapped]
        public string FullName 
        {
            get
            {
                string full = $"{LastName} {FirstName}";
                if (!string.IsNullOrWhiteSpace(Patronymic))
                    full += $" {Patronymic}";
                return full;
            }
        }
    }
}
