using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp02.Models;
using WebApp02.ViewModel;
using System.Linq;

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
        [HttpGet]
        public IActionResult PublishingHouse()
        {
            InsertPublishingHouseViewModel model = new()
            {
                PublishingHouse = new PublishingHouse(),
                AllPublishingHouses = _db.PublishingHouses.AsEnumerable()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult PublishingHouse(InsertPublishingHouseViewModel model)
        {
            if (ModelState.IsValid)
            {
                _db.PublishingHouses.Add(model.PublishingHouse);
                _db.SaveChanges();
                model = new()
                {
                    PublishingHouse = new PublishingHouse()
                };
                //return RedirectToAction("Index", "Home");
            }
            model.AllPublishingHouses = _db.PublishingHouses.AsEnumerable();
            return View(model);
        }
        [HttpGet]
        public IActionResult Autor()
        {
            InsertAutorViewModel vm = new()
            {
                Autor = new Autor(),
                ListAutors = _db.Autors.AsEnumerable()
            };
            GetAutorsPage(ref vm);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Autor(InsertAutorViewModel vm, int page = 1)
        {
            var autors = _db.Autors.AsEnumerable();

            vm.Autor ??= new Autor();
            vm.Autor.BirthDate = vm.Autor.BirthDate.ToUniversalTime();
            bool isExist = autors.Any(x => 
                x.FirstName == vm.Autor.FirstName &&
                x.LastName == vm.Autor.LastName &&
                x.Patronymic == vm.Autor.Patronymic &&
                x.BirthDate == vm.Autor.BirthDate);
            if (isExist)
                ModelState.AddModelError("Autor", "Такой автор уже существует!");
            if(ModelState.IsValid)
            {
                _db.Autors.Add(vm.Autor);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            vm.ListAutors = autors;
            GetAutorsPage(ref vm, page);
            //vm.Books = new SelectList(booklist, "Id", "Title", booklist.Where(x => vm.BookIds.Contains(x.Id)));
            return View(vm);
        }
        public IActionResult ListAutorsPartial(/*string page = "1"*/ int page = 1)
        {
            //int pageSize = 2;
            //IQueryable<Autor> source = _db.Autors;
            //var count = source.Count();
            //var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            //PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            //InsertAutorViewModel vm = new()
            //{
            //    //ListAutors = _db.Autors.AsEnumerable()
            //    Page = pageViewModel,
            //    ListAutors = items
            //};
            InsertAutorViewModel vm = new InsertAutorViewModel();
            /*_ = int.TryParse(page, out int number);
            if (number == 0)
                number = 1;
            GetAutorsPage(ref vm, number);*/
            GetAutorsPage(ref vm, page);
            return PartialView(vm);
        }
        private void GetAutorsPage(ref InsertAutorViewModel vm, int page = 1)
        {
            int pageSize = 1;
            IQueryable<Autor> source = _db.Autors;
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new(count, page, pageSize);
            vm.Page = pageViewModel;
            vm.ListAutors = items;
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
            //vm.AutorsIds ??= new List<int>();
            //vm.GenresIds ??= new List<int>();
            /*vm.PublishingHouseSelected = vm.PublishingHouseId;
            vm.AutorsSelected = vm.AutorsIds ?? new List<int>();
            vm.GenresSelected = vm.GenresIds ?? new List<int>();*/
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
    }
}
