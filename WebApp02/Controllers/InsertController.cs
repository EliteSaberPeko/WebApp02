using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp02.Models;
using WebApp02.ViewModel;
using System.Linq;
using WebApp02.Utils;
using Microsoft.EntityFrameworkCore;

namespace WebApp02.Controllers
{
    public class InsertController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _db;
        public InsertController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _db = context;
        }

        #region PublishingHouse
        [HttpGet]
        public IActionResult PublishingHouse()
        {
            var model = Pages.GetBaseViewModel(_db.PublishingHouses.AsQueryable(), 1);
            return View(model);
        }
        [HttpPost]
        public IActionResult PublishingHouse(BaseInsertViewModel<PublishingHouse> vm)
        {
            vm = Database.PublishingHouseInsert(vm, _db, ModelState);
            if (ModelState.IsValid)
                return RedirectToAction("Index", "Home");
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
        public IActionResult Autor()
        {
            var vm = Pages.GetBaseViewModel(_db.Autors.AsQueryable(), 1);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Autor(BaseInsertViewModel<Autor> vm)
        {
            vm = Database.AutorInsert(vm, _db, ModelState, 1);
            if (ModelState.IsValid)
                return RedirectToAction("Index", "Home");
            return View(vm);
        }
        public IActionResult ListAutorsPartial(int page = 1)
        {
            IQueryable<Autor> source = _db.Autors;
            var vm = Pages.GetBaseViewModel(source, page);
            return PartialView(vm);
        }
        #endregion

        #region Book
        [HttpGet]
        public IActionResult Book()
        {
            PageViewModel page = Pages.GetPageViewModelAndItems(_db.Books.AsQueryable(), 1, out var items);
            InsertBookViewModel vm = new()
            {
                Item = new(),
                ListItems = items.Include(x => x.Autors),
                Page = page
            };
            GetInsertBookViewModel(ref vm, _db);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Book(InsertBookViewModel vm)
        {
            vm = Database.BookInsert(vm, _db, ModelState, 1);
            if (ModelState.IsValid)
                return RedirectToAction("Index", "Home");
            return View(vm);
        }
        public IActionResult ListBooksPartial(int page = 1)
        {
            InsertBookViewModel vm = new();
            IQueryable<Book> source = _db.Books;
            vm.Page = Pages.GetPageViewModelAndItems(source, page, out var items);
            vm.ListItems = items.Include(x => x.Autors);
            GetInsertBookViewModel(ref vm, _db);
            return PartialView(vm);
        }
        #region Поиск автора
        public IActionResult SearchAutor(string[] data)
        {
            var viewModel = GetViewModelForSearchAutor(data, _db);
            return PartialView("_book", viewModel);
        }
        public static InsertBookViewModel GetViewModelForSearchAutor(string[] data, ApplicationContext db)
        {
            var genres = db.Genres.AsEnumerable();
            var autors = db.Autors.AsEnumerable();
            var houses = db.PublishingHouses.AsEnumerable();
            List<Autor> result = new();

            string searchAutor = data[0] ?? string.Empty,
                title = data[1] ?? string.Empty;
            _ = int.TryParse(data[2] ?? "0", out int pubHouse);
            _ = uint.TryParse(data[3] ?? "0", out uint year);
            string description = data[4] ?? string.Empty;
            _ = uint.TryParse(data[5] ?? "0", out uint count);
            _ = decimal.TryParse((data[6] ?? "0").Replace('.', ','), out decimal price);
            IEnumerable<int> autorsIds = (data[7] ?? "0").Split('|').AsEnumerable().Select(x => int.Parse(x));
            IEnumerable<int> genreIds = (data[8] ?? "0").Split('|').AsEnumerable().Select(x => int.Parse(x));

            IEnumerable<Autor> selectedAutors = autors.Where(x => autorsIds.Contains(x.Id));
            if (!string.IsNullOrWhiteSpace(searchAutor))
                result = result.Union(autors.Where(x => x.FullName.ToUpper().Contains(searchAutor.ToUpper()))).ToList();
            result = result.Union(selectedAutors).ToList();
            if (!result.Any())
                result = autors.ToList();

            IEnumerable<Genre> selectedGenres = genres.Where(x => genreIds.Contains(x.Id));
            Book book = new()
            {
                Title = title,
                Description = description,
                Price = price,
                Count = count,
                PublicationYear = year
            };
            InsertBookViewModel viewModel = new()
            {
                Item = book,
                Autors = result.ToDictionary(key => key.Id, val => val.FullName),
                Genres = genres.ToDictionary(key => key.Id, val => val.Name),
                AutorsIds = selectedAutors.Select(x => x.Id),
                GenresIds = selectedGenres.Select(x => x.Id),
                PublishingHouses = houses.ToDictionary(key => key.Id, val => val.Name),
                PublishingHouseId = pubHouse
            };
            return viewModel;
        }
        #endregion
        public static InsertBookViewModel GetInsertBookViewModel(ref InsertBookViewModel vm, ApplicationContext db)
        {
            IEnumerable<PublishingHouse> pubHouses = db.PublishingHouses.AsEnumerable();
            IEnumerable<Autor> autors = db.Autors.AsEnumerable();
            IEnumerable<Genre> genres = db.Genres.AsEnumerable();
            vm.PublishingHouses = pubHouses.ToDictionary(key => key.Id, val => val.Name);
            vm.Autors = autors.ToDictionary(key => key.Id, val => $"{val.FullName} | {val.BirthDate.ToLongDateString()}");
            vm.Genres = genres.ToDictionary(key => key.Id, val => val.Name);
            vm.AutorsIds ??= new List<int>();
            vm.GenresIds ??= new List<int>();
            //vm.PublishingHouseId = 0;
            return vm;
        }
        #endregion

        #region Genre
        [HttpGet]
        public IActionResult Genre()
        {
            var model = Pages.GetBaseViewModel(_db.Genres.AsQueryable(), 1);
            return View(model);
        }
        [HttpPost]
        public IActionResult Genre(BaseInsertViewModel<Genre> vm)
        {
            vm = Database.GenreInsert(vm, _db, ModelState, 1);
            if (ModelState.IsValid)
                return RedirectToAction("Index", "Home");
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
