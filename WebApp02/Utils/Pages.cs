using WebApp02.Interfaces;
using WebApp02.ViewModel;

namespace WebApp02.Utils
{
    public static class Pages
    {
        public static PageViewModel GetPageViewModelAndItems<T>(IQueryable<T> source, int page, out List<T> items, int pageSize = 1) where T : IModel
        {
            var count = source.Count();
            items = source.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new(count, page, pageSize);
            return pageViewModel;
        }
        public static BaseInsertViewModel<T> GetBaseViewModel<T>(IQueryable<T> source, int page, int pageSize = 1) where T : IModel, new()
        {
            PageViewModel pageViewModel = GetPageViewModelAndItems(source, page, out var items, pageSize);
            BaseInsertViewModel<T> model = new()
            {
                Item = new(),
                ListItems = items,
                Page = pageViewModel
            };
            return model;
        }
    }
}
