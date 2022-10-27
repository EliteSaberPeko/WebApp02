using System.ComponentModel.DataAnnotations;
using WebApp02.Interfaces;
namespace WebApp02.Models
{
    public class PublishingHouse : IModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название обязательно для заполнения")]
        [Display(Name = "Название")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "В названии допустимо от 1 до 100 символов")]
        public string Name { get; set; }
    }
}
