using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp02.Models;
using WebApp02.ViewModel;
using System.Linq;
using WebApp02.Utils;

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
            PageViewModel page = Pages.GetPageViewModelAndItems(_db.PublishingHouses.AsQueryable(), 1, out var items);
            InsertPublishingHouseViewModel model = new()
            {
                PublishingHouse = new PublishingHouse(),
                ListPublishingHouses = items,
                Page = page
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult PublishingHouse(InsertPublishingHouseViewModel vm, int page = 1)
        {
            var pubHouses = _db.PublishingHouses.AsQueryable();
            if (ModelState.IsValid)
            {
                _db.PublishingHouses.Add(vm.PublishingHouse);
                _db.SaveChanges();
                vm = new()
                {
                    PublishingHouse = new PublishingHouse()
                };
                //return RedirectToAction("Index", "Home");
            }
            vm.Page = Pages.GetPageViewModelAndItems(pubHouses, page, out var items);
            vm.ListPublishingHouses = items;
            return View(vm);
        }
        public IActionResult ListPublishingHousesPartial(int page = 1)
        {
            InsertPublishingHouseViewModel vm = new();
            IQueryable<PublishingHouse> source = _db.PublishingHouses;
            vm.Page = Pages.GetPageViewModelAndItems(source, page, out var items);
            vm.ListPublishingHouses = items;
            return PartialView(vm);
        } 
        #endregion

        #region Autor
        [HttpGet]
        public IActionResult Autor()
        {
            PageViewModel page = Pages.GetPageViewModelAndItems(_db.Autors.AsQueryable(), 1, out List<Autor> items);
            InsertAutorViewModel vm = new()
            {
                Autor = new Autor(),
                ListAutors = items,
                Page = page
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Autor(InsertAutorViewModel vm, int page = 1)
        {
            var autors = _db.Autors.AsQueryable();

            vm.Autor ??= new Autor();
            vm.Autor.BirthDate = vm.Autor.BirthDate.ToUniversalTime();
            bool isExist = autors.Any(x =>
                x.FirstName == vm.Autor.FirstName &&
                x.LastName == vm.Autor.LastName &&
                x.Patronymic == vm.Autor.Patronymic &&
                x.BirthDate == vm.Autor.BirthDate);
            if (isExist)
                ModelState.AddModelError("Autor", "Такой автор уже существует!");
            if (ModelState.IsValid)
            {
                _db.Autors.Add(vm.Autor);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            vm.Page = Pages.GetPageViewModelAndItems(autors, page, out var items);
            vm.ListAutors = items;
            return View(vm);
        }
        public IActionResult ListAutorsPartial(int page = 1)
        {
            InsertAutorViewModel vm = new InsertAutorViewModel();
            IQueryable<Autor> source = _db.Autors;
            vm.Page = Pages.GetPageViewModelAndItems(source, page, out var items);
            vm.ListAutors = items;
            return PartialView(vm);
        }
        private InsertBookViewModel GetInsertBookViewModel(ref InsertBookViewModel vm)
        {
            IEnumerable<PublishingHouse> pubHouses = _db.PublishingHouses.AsEnumerable();
            IEnumerable<Autor> autors = _db.Autors.AsEnumerable();
            IEnumerable<Genre> genres = _db.Genres.AsEnumerable();
            vm.PublishingHouses = pubHouses.ToDictionary(key => key.Id, val => val.Name);
            vm.Autors = autors.ToDictionary(key => key.Id, val => val.FullName);
            vm.Genres = genres.ToDictionary(key => key.Id, val => val.Name);
            vm.AutorsIds ??= new List<int>();
            vm.GenresIds ??= new List<int>();
            //vm.PublishingHouseId = 0;
            return vm;
        }
        #endregion

        #region Book
        [HttpGet]
        public IActionResult Book()
        {
            InsertBookViewModel vm = new InsertBookViewModel();
            GetInsertBookViewModel(ref vm);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Book(InsertBookViewModel vm)
        {

            if (ModelState.IsValid)
            {
                PublishingHouse ph = _db.PublishingHouses.FirstOrDefault(x => x.Id == vm.PublishingHouseId);
                List<Autor> autors = _db.Autors.Where(x => vm.AutorsIds.Contains(x.Id)).ToList();
                List<Genre> genres = _db.Genres.Where(x => vm.GenresIds.Contains(x.Id)).ToList();
                vm.Book.PublishingHouse = ph;
                vm.Book.Autors = autors;
                vm.Book.Genres = genres;
                _db.Books.Add(vm.Book);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            GetInsertBookViewModel(ref vm);
            return View(vm);
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
                Book = book,
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
            PageViewModel page = Pages.GetPageViewModelAndItems(_db.Genres.AsQueryable(), 1, out var items);
            BaseInsertViewModel<Genre> model = new()
            {
                Item = new(),
                ListItems = items,
                Page = page
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Genre(BaseInsertViewModel<Genre> vm, int page = 1)
        {
            var genres = _db.Genres.AsQueryable();

            vm.Item ??= new();
            vm.Item.Name = vm.Item.Name.Trim();
            bool isExist = genres.Any(x => x.Name == vm.Item.Name);
            if (isExist)
                ModelState.AddModelError("Genre", "Такой жанр уже существует!");

            if (ModelState.IsValid)
            {
                _db.Genres.Add(vm.Item);
                _db.SaveChanges();
                vm = new()
                {
                    Item = new()
                };
            }
            vm.Page = Pages.GetPageViewModelAndItems(genres, page, out var items);
            vm.ListItems = items;
            return View(vm);
        }
        public IActionResult ListGenresPartial(int page = 1)
        {
            BaseInsertViewModel<Genre> vm = new();
            IQueryable<Genre> source = _db.Genres;
            vm.Page = Pages.GetPageViewModelAndItems(source, page, out var items);
            vm.ListItems = items;
            return PartialView(vm);
        }
        #endregion
    }
}
