using WebApp02.Models;

namespace WebApp02.ViewModel
{
    public class InsertAutorViewModel
    {
        public Autor Autor { get; set; }
        public IEnumerable<Autor> ListAutors { get; set; } = new List<Autor>();
        public PageViewModel Page { get; set; }
    }
}
