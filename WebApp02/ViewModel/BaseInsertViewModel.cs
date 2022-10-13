using WebApp02.Models;

namespace WebApp02.ViewModel
{
    public class BaseInsertViewModel<T>
    {
        public T Item { get; set; }
        public IEnumerable<T> ListItems { get; set; } = new List<T>();
        public PageViewModel Page { get; set; }
    }
}
