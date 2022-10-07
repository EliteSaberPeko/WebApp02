using WebApp02.ViewModel;

namespace WebApp02.Utils
{
    public static class Pages
    {
        public static PageViewModel GetPageViewModelAndItems<T>(IQueryable<T> source, int page, out List<T> items, int pageSize = 1)
        {
            var count = source.Count();
            items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new(count, page, pageSize);
            return pageViewModel;
        }
    }
}
