using Microsoft.AspNetCore.Mvc;
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
        public IActionResult PublishingHouse()
        {
            var model = Pages.GetBaseViewModel(_db.PublishingHouses.AsQueryable(), 1, 10);
            return View(model);
        }
        public IActionResult Autor()
        {
            var model = Pages.GetBaseViewModel(_db.Autors.AsQueryable(), 1, 10);
            return View(model);
        }
        public IActionResult Genre()
        {
            var model = Pages.GetBaseViewModel(_db.Genres.AsQueryable(), 1, 10);
            return View(model);
        }
    }
}
