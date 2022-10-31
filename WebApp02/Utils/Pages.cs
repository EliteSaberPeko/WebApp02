using WebApp02.Interfaces;
using WebApp02.ViewModel;

namespace WebApp02.Utils
{
    public static class Pages
    {
        public static PageViewModel GetPageViewModelAndItems<T>(IQueryable<T> source, int page, out IQueryable<T> items, int pageSize = 5) where T : IModel
        {
            var count = source.Count();
            items = source.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new(count, page, pageSize);
            return pageViewModel;
        }
        public static BaseInsertViewModel<T> GetBaseViewModel<T>(IQueryable<T> source, int page, int pageSize = 5) where T : IModel, new()
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
