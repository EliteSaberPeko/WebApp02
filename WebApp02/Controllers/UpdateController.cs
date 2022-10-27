using Microsoft.AspNetCore.Mvc;
using WebApp02.Models;
using WebApp02.Utils;
using WebApp02.ViewModel;

namespace WebApp02.Controllers
{
    public class UpdateController : Controller
    {
        private ApplicationContext _db;
        public UpdateController(ApplicationContext context)
        {
            _db = context;
        }

        #region PublishingHouse
        [HttpGet]
        public IActionResult PublishingHouse(int id)
        {
            var source = _db.PublishingHouses.AsQueryable();
            var vm = Pages.GetBaseViewModel(source, 1);
            var pubHouse = _db.PublishingHouses.FirstOrDefault(x => x.Id == id);
            if (pubHouse == null)
                return RedirectToAction("PublishingHouse", "List");
            else
                vm.Item = pubHouse;
            return View(vm);
        }
        [HttpPost]
        public IActionResult PublishingHouse(BaseInsertViewModel<PublishingHouse> vm)
        {
            vm = Database.PublishingHouseUpdate(vm, _db, ModelState, 1);
            if (ModelState.IsValid)
                return RedirectToAction("PublishingHouse", "List");
            return View(vm);
        }
        public IActionResult ListPublishingHousesPartial(int page = 1)
        {
            IQueryable<PublishingHouse> source = _db.PublishingHouses;
            var vm = Pages.GetBaseViewModel(source, page);
            return PartialView(vm);
        }
        #endregion

        #region Autor
        [HttpGet]
        public IActionResult Autor(int id)
        {
            var source = _db.Autors.AsQueryable();
            var vm = Pages.GetBaseViewModel(source, 1);
            var item = _db.Autors.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return RedirectToAction("Autor", "List");
            else
                vm.Item = item;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Autor(BaseInsertViewModel<Autor> vm)
        {
            vm = Database.AutorUpdate(vm, _db, ModelState, 1);
            if (ModelState.IsValid)
                return RedirectToAction("Autor", "List");
            return View(vm);
        }
        public IActionResult ListAutorsPartial(int page = 1)
        {
            IQueryable<Autor> source = _db.Autors;
            var vm = Pages.GetBaseViewModel(source, page);
            return PartialView(vm);
        }
        #endregion

        #region Genre
        [HttpGet]
        public IActionResult Genre(int id)
        {
            var source = _db.Genres.AsQueryable();
            var vm = Pages.GetBaseViewModel(source, 1);
            var item = _db.Genres.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return RedirectToAction("Genre", "List");
            else
                vm.Item = item;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Genre(BaseInsertViewModel<Genre> vm)
        {
            vm = Database.GenreUpdate(vm, _db, ModelState, 1);
            if (ModelState.IsValid)
                return RedirectToAction("Genre", "List");
            return View(vm);
        }
        public IActionResult ListGenresPartial(int page = 1)
        {
            IQueryable<Genre> source = _db.Genres;
            var vm = Pages.GetBaseViewModel(source, page);
            return PartialView(vm);
        }
        #endregion
    }
}
