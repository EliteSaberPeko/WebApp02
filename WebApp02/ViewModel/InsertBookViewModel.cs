using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebApp02.Models;

namespace WebApp02.ViewModel
{
    public class InsertBookViewModel : BaseInsertViewModel<Book>
    {
        //public Book Book { get; set; } = new Book();
        /*public IEnumerable<PublishingHouse> PublishingHouses { get; set; }
        public IEnumerable<Autor> Autors { get; set; }
        public IEnumerable<Genre> Genres { get; set; }*/
        public Dictionary<int, string>? PublishingHouses { get; set; }
        public Dictionary<int, string>? Autors { get; set; }
        public Dictionary<int, string>? Genres { get; set; }
        [Required(ErrorMessage = "Необходим минимум 1 автор")]
        public IEnumerable<int> AutorsIds { get; set; }
        [Required(ErrorMessage = "Необходим минимум 1 жанр")]
        public IEnumerable<int> GenresIds { get; set; }
        [Range(1, 9999999, ErrorMessage = "Издательство необходимо")]
        public int PublishingHouseId { get; set; }

        /*public int PublishingHouseSelected { get; set; } = 0;
        public IEnumerable<int> AutorsSelected { get; set; } = new List<int>();
        public IEnumerable<int> GenresSelected { get; set; } = new List<int>();*/
    }
}
