using Microsoft.AspNetCore.Mvc;
using WebApp02.Models;
using WebApp02.Utils;

namespace WebApp02.Controllers
{
    public class DeleteController : Controller
    {
        private ApplicationContext _db;
        public DeleteController(ApplicationContext context)
        {
            _db = context;
        }
        [HttpPost]
        public IActionResult PublishingHouse(int id = 0)
        {
            Database.PublishingHouseDelete(_db, id);
            return RedirectToAction("PublishingHouse", "List");
        }
        [HttpPost]
        public IActionResult Autor(int id = 0)
        {
            Database.AutorDelete(_db, id);
            return RedirectToAction("Autor", "List");
        }
        [HttpPost]
        public IActionResult Book(int id = 0)
        {
            Database.BookDelete(_db, id);
            return RedirectToAction("Book", "List");
        }
        [HttpPost]
        public IActionResult Genre(int id = 0)
        {
            Database.GenreDelete(_db, id);
            return RedirectToAction("Genre", "List");
        }
    }
}
