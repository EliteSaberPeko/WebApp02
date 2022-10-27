using WebApp02.Interfaces;
namespace WebApp02.Models
{
    public class Genre : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
