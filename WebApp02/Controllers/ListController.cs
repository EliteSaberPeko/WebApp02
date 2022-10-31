using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp02.Models;
using WebApp02.Utils;
using WebApp02.ViewModel;

namespace WebApp02.Controllers
{
    public class ListController : Controller
    {
        private ApplicationContext _db;
        public ListController(ApplicationContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region PublishingHouse
        public IActionResult PublishingHouse()
        {
            var model = Pages.GetBaseViewModel(_db.PublishingHouses.AsQueryable(), 1);
            return View(model);
        }
        public IActionResult PublishingHousePage(int page = 1)
        {
            var model = Pages.GetBaseViewModel(_db.PublishingHouses.AsQueryable(), page);
            return PartialView(model);
        }
        #endregion

        #region Autor
        public IActionResult Autor()
        {
            var model = Pages.GetBaseViewModel(_db.Autors.AsQueryable(), 1);
            return View(model);
        }
        public IActionResult AutorPage(int page = 1)
        {
            var model = Pages.GetBaseViewModel(_db.Autors.AsQueryable(), page);
            return PartialView(model);
        }
        #endregion

        #region Genre
        public IActionResult Genre()
        {
            var model = Pages.GetBaseViewModel(_db.Genres.AsQueryable(), 1);
            return View(model);
        }
        public IActionResult GenrePage(int page = 1)
        {
            var model = Pages.GetBaseViewModel(_db.Genres.AsQueryable(), page);
            return PartialView(model);
        }
        #endregion

        #region Book
        public IActionResult Book()
        {
            PageViewModel page = Pages.GetPageViewModelAndItems(_db.Books.AsQueryable(), 1, out var items);
            BaseInsertViewModel<Book> vm = new()
            {
                Item = new(),
                ListItems = items.Include(x => x.Autors).Include(x => x.PublishingHouse),
                Page = page
            };
            return View(vm);
        }
        public IActionResult BookPage(int page = 1)
        {
            PageViewModel pageViewModel = Pages.GetPageViewModelAndItems(_db.Books.AsQueryable(), page, out var items);
            BaseInsertViewModel<Book> vm = new()
            {
                Item = new(),
                ListItems = items.Include(x => x.Autors).Include(x => x.PublishingHouse),
                Page = pageViewModel
            };
            return PartialView(vm);
        } 
        #endregion
    }
}
