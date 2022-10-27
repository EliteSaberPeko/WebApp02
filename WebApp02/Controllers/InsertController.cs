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
                ListItems = items,
                Page = page
            };
            //var vm = (InsertBookViewModel)Pages.GetBaseViewModel(_db.Books.AsQueryable(), 1);
            //InsertBookViewModel vm = new();
            GetInsertBookViewModel(ref vm);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Book(InsertBookViewModel vm, int page = 1)
        {
            var books = _db.Books.AsQueryable();
            
            vm.Item ??= new();
            vm.Item.Title = vm.Item.Title?.Trim();
            vm.Item.Description = vm.Item.Description?.Trim();
            bool isExist = books.Any(x =>
                x.Title == vm.Item.Title &&
                x.Description == vm.Item.Description &&
                x.PublicationYear == vm.Item.PublicationYear);
            if (isExist)
                ModelState.AddModelError("Autor", "Такой автор уже существует!");

            if (ModelState.IsValid)
            {
                List<Autor> autors = _db.Autors.Where(x => vm.AutorsIds.Contains(x.Id)).ToList();
                List<Genre> genres = _db.Genres.Where(x => vm.GenresIds.Contains(x.Id)).ToList();
                PublishingHouse ph = _db.PublishingHouses.FirstOrDefault(x => x.Id == vm.PublishingHouseId);
                vm.Item.PublishingHouse = ph;
                vm.Item.Autors = autors;
                vm.Item.Genres = genres;
                _db.Books.Add(vm.Item);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            vm.Page = Pages.GetPageViewModelAndItems(books, page, out var items);
            vm.ListItems = items;
            GetInsertBookViewModel(ref vm);
            return View(vm);
        }
        private InsertBookViewModel GetInsertBookViewModel(ref InsertBookViewModel vm)
        {
            IEnumerable<PublishingHouse> pubHouses = _db.PublishingHouses.AsEnumerable();
            IEnumerable<Autor> autors = _db.Autors.AsEnumerable();
            IEnumerable<Genre> genres = _db.Genres.AsEnumerable();
            vm.PublishingHouses = pubHouses.ToDictionary(key => key.Id, val => val.Name);
            vm.Autors = autors.ToDictionary(key => key.Id, val => $"{val.FullName} | {val.BirthDate.ToLongDateString()}");
            vm.Genres = genres.ToDictionary(key => key.Id, val => val.Name);
            vm.AutorsIds ??= new List<int>();
            vm.GenresIds ??= new List<int>();
            //vm.PublishingHouseId = 0;
            return vm;
        }
        public IActionResult ListBooksPartial(int page = 1)
        {
            InsertBookViewModel vm = new();
            IQueryable<Book> source = _db.Books;
            vm.Page = Pages.GetPageViewModelAndItems(source, page, out var items);
            vm.ListItems = items;
            //var vm = Pages.GetBaseViewModel(source, page) as InsertBookViewModel;
            GetInsertBookViewModel(ref vm);
            return PartialView(vm);
        }
        #region Поиск автора
        public IActionResult SearchAutor(string[] data)
        {
            var genres = _db.Genres.AsEnumerable();
            var autors = _db.Autors.AsEnumerable();
            var houses = _db.PublishingHouses.AsEnumerable();
            List<Autor> result = new();

            string searchAutor = data[0] ?? string.Empty,
                title = data[1] ?? string.Empty;
            //int pubHouse = int.Parse(data[2] ?? "0");
            _ = int.TryParse(data[2] ?? "0", out int pubHouse);
            _ = uint.TryParse(data[3] ?? "0", out uint year);
            //uint year = uint.Parse(data[3] ?? "0");
            string description = data[4] ?? string.Empty;
            //uint count = uint.Parse(data[5] ?? "0");
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
                //AutorsSelected = selectedAutors.Select(x => x.Id),
                //GenresSelected = selectedGenres.Select(x => x.Id),
                AutorsIds = selectedAutors.Select(x => x.Id),
                GenresIds = selectedGenres.Select(x => x.Id),
                PublishingHouses = houses.ToDictionary(key => key.Id, val => val.Name),
                //PublishingHouseSelected = pubHouse
                PublishingHouseId = pubHouse
            };
            return PartialView("_book", viewModel);
        }
        #endregion
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
