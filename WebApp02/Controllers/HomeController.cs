using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WebApp02.Models;
//using WebApp02.ViewModel;

namespace WebApp02.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _db = context;
        }

        public IActionResult Index()
        {
            List<Autor> autors = new List<Autor>();
            autors.AddRange(_db.Autors.ToList());
            /*Autor autor = new Autor()
            {
                Id = 1,
                FirstName = "Иван",
                LastName = "Иванов",
                Patronymic = "Иванович",
                Books = new List<Book>()
            };
            autors.Add(autor);*/
            //if (!string.IsNullOrWhiteSpace(Request.Query["name"]))
                //autor.FirstName = Request.Query["name"];
            return View(autors);
        }
        [HttpPost]
        public IActionResult Index(IEnumerable<Autor> autors)
        {
            return View(autors);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}