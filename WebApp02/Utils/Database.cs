using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebApp02.Controllers;
using WebApp02.Models;
using WebApp02.ViewModel;
using WebApp02.Interfaces;

namespace WebApp02.Utils
{
    public static class Database
    {
        #region PublishingHouse
        public static BaseInsertViewModel<PublishingHouse> PublishingHouseUpdate(BaseInsertViewModel<PublishingHouse> vm, ApplicationContext db, ModelStateDictionary modelState, int page = 1)
        {
            return PublishingHouse(vm, db, modelState, false, page);
        }
        public static BaseInsertViewModel<PublishingHouse> PublishingHouseInsert(BaseInsertViewModel<PublishingHouse> vm, ApplicationContext db, ModelStateDictionary modelState, int page = 1)
        {
            return PublishingHouse(vm, db, modelState, true, page);
        }
        public static void PublishingHouseDelete(ApplicationContext db, int id) => Delete(db, db.PublishingHouses, id);
        private static BaseInsertViewModel<PublishingHouse> PublishingHouse(BaseInsertViewModel<PublishingHouse> vm, ApplicationContext db, ModelStateDictionary modelState, bool insert, int page = 1)
        {
            var pubHouses = db.PublishingHouses.AsQueryable();

            vm.Item ??= new();
            vm.Item.Name = vm.Item.Name?.Trim();
            bool isExist = pubHouses.Any(x => x.Name == vm.Item.Name);

            if (isExist)
                modelState.AddModelError("PublishingHouse", "Такой издатель уже существует!");

            if (!insert && !pubHouses.Any(x => x.Id == vm.Item.Id))
                modelState.AddModelError("PublishingHouse Update", "Издательство для редактирования не найдено!");

            if (modelState.IsValid)
            {
                if (insert)
                {
                    db.PublishingHouses.Add(vm.Item);
                }
                else
                {
                    var instance = pubHouses.First(x => x.Id == vm.Item.Id);
                    instance.Name = vm.Item.Name;
                    db.PublishingHouses.Update(instance);
                }

                db.SaveChanges();
            }
            vm.Page = Pages.GetPageViewModelAndItems(pubHouses, page, out var items);
            vm.ListItems = items;
            return vm;
        }
        #endregion

        #region Autor
        public static BaseInsertViewModel<Autor> AutorUpdate(BaseInsertViewModel<Autor> vm, ApplicationContext db, ModelStateDictionary modelState, int page = 1)
        {
            return Autor(vm, db, modelState, false, page);
        }
        public static BaseInsertViewModel<Autor> AutorInsert(BaseInsertViewModel<Autor> vm, ApplicationContext db, ModelStateDictionary modelState, int page = 1)
        {
            return Autor(vm, db, modelState, true, page);
        }
        public static void AutorDelete(ApplicationContext db, int id) => Delete(db, db.Autors, id);
        private static BaseInsertViewModel<Autor> Autor(BaseInsertViewModel<Autor> vm, ApplicationContext db, ModelStateDictionary modelState, bool insert, int page = 1)
        {
            var autors = db.Autors.AsQueryable();

            vm.Item ??= new();
            vm.Item.BirthDate = vm.Item.BirthDate.AddDays(1);
            vm.Item.BirthDate = vm.Item.BirthDate.ToUniversalTime();
            vm.Item.FirstName = vm.Item.FirstName?.Trim();
            vm.Item.LastName = vm.Item.LastName?.Trim();
            vm.Item.Patronymic = vm.Item.Patronymic?.Trim();
            bool isExist = autors.Any(x =>
                x.FirstName == vm.Item.FirstName &&
                x.LastName == vm.Item.LastName &&
                x.Patronymic == vm.Item.Patronymic &&
                x.BirthDate == vm.Item.BirthDate);

            if (isExist)
                modelState.AddModelError("Autor", "Такой автор уже существует!");

            if (!insert && !autors.Any(x => x.Id == vm.Item.Id))
                modelState.AddModelError("Autor Update", "Автор для редактирования не найден!");

            if (modelState.IsValid)
            {
                if (insert)
                {
                    db.Autors.Add(vm.Item);
                }
                else
                {
                    var instance = autors.First(x => x.Id == vm.Item.Id);
                    instance.FirstName = vm.Item.FirstName;
                    instance.LastName = vm.Item.LastName;
                    instance.Patronymic = vm.Item.Patronymic;
                    instance.BirthDate = vm.Item.BirthDate;
                    db.Autors.Update(instance);
                }

                db.SaveChanges();
            }
            vm.Page = Pages.GetPageViewModelAndItems(autors, page, out var items);
            vm.ListItems = items;
            return vm;
        }
        #endregion

        #region Book
        public static InsertBookViewModel BookUpdate(InsertBookViewModel vm, ApplicationContext db, ModelStateDictionary modelState, int page = 1)
        {
            return Book(vm, db, modelState, false, page);
        }
        public static InsertBookViewModel BookInsert(InsertBookViewModel vm, ApplicationContext db, ModelStateDictionary modelState, int page = 1)
        {
            return Book(vm, db, modelState, true, page);
        }
        public static void BookDelete(ApplicationContext db, int id) => Delete(db, db.Books, id);
        private static InsertBookViewModel Book(InsertBookViewModel vm, ApplicationContext db, ModelStateDictionary modelState, bool insert, int page = 1)
        {
            var books = db.Books.AsQueryable();

            vm.Item ??= new();
            vm.AutorsIds ??= new List<int>();
            vm.GenresIds ??= new List<int>();
            vm.Item.Title = vm.Item.Title?.Trim();
            vm.Item.Description = vm.Item.Description?.Trim();
            bool isExist = books.Any(x =>
                x.Title == vm.Item.Title &&
                x.Description == vm.Item.Description &&
                x.PublicationYear == vm.Item.PublicationYear &&
                (x.PublishingHouse != null && x.PublishingHouse.Id == vm.PublishingHouseId) &&
                (x.Autors.Count == vm.AutorsIds.Count() && x.Autors.All(x => vm.AutorsIds.Contains(x.Id))) &&
                (x.Genres.Count == vm.GenresIds.Count() && x.Genres.All(x => vm.GenresIds.Contains(x.Id))));

            if (isExist)
                modelState.AddModelError("Book", "Такая книга уже существует!");

            if(!insert && !books.Any(x => x.Id == vm.Item.Id))
                modelState.AddModelError("Book Update", "Книга для редактирования не найдена!");

            if (modelState.IsValid)
            {
                List<Autor> autors = db.Autors.Where(x => vm.AutorsIds.Contains(x.Id)).OrderBy(x => x.Id).ToList();
                List<Genre> genres = db.Genres.Where(x => vm.GenresIds.Contains(x.Id)).OrderBy(x => x.Id).ToList();
                PublishingHouse ph = db.PublishingHouses.FirstOrDefault(x => x.Id == vm.PublishingHouseId);
                vm.Item.PublishingHouse = ph;
                vm.Item.Autors = autors;
                vm.Item.Genres = genres;
                if (insert)
                {
                    db.Books.Add(vm.Item);
                }
                else
                {
                    var instance = books.Include(x => x.Autors).Include(x => x.Genres).First(x => x.Id == vm.Item.Id);
                    instance.Autors = autors;
                    instance.Genres = genres;
                    instance.PublishingHouse = ph;
                    instance.Description = vm.Item.Description;
                    instance.Count = vm.Item.Count;
                    instance.Title = vm.Item.Title;
                    instance.Price = vm.Item.Price;
                    instance.PublicationYear = vm.Item.PublicationYear;
                    db.Books.Update(instance);
                }
                db.SaveChanges();
            }
            vm.Page = Pages.GetPageViewModelAndItems(books, page, out var items);
            vm.ListItems = items;
            InsertController.GetInsertBookViewModel(ref vm, db);
            return vm;
        }
        #endregion

        #region Genre
        public static BaseInsertViewModel<Genre> GenreUpdate(BaseInsertViewModel<Genre> vm, ApplicationContext db, ModelStateDictionary modelState, int page = 1)
        {
            return Genre(vm, db, modelState, false, page);
        }
        public static BaseInsertViewModel<Genre> GenreInsert(BaseInsertViewModel<Genre> vm, ApplicationContext db, ModelStateDictionary modelState, int page = 1)
        {
            return Genre(vm, db, modelState, true, page);
        }
        public static void GenreDelete(ApplicationContext db, int id) => Delete(db, db.Genres, id);
        private static BaseInsertViewModel<Genre> Genre(BaseInsertViewModel<Genre> vm, ApplicationContext db, ModelStateDictionary modelState, bool insert, int page = 1)
        {
            var genres = db.Genres.AsQueryable();

            vm.Item ??= new();
            vm.Item.Name = vm.Item.Name?.Trim();
            bool isExist = genres.Any(x => x.Name == vm.Item.Name);
            if (isExist)
                modelState.AddModelError("Genre", "Такой жанр уже существует!");

            if (!insert && !genres.Any(x => x.Id == vm.Item.Id))
                modelState.AddModelError("Genre Update", "Жанр для редактирования не найден!");

            if (modelState.IsValid)
            {
                if (insert)
                {
                    db.Genres.Add(vm.Item);
                }
                else
                {
                    var instance = genres.First(x => x.Id == vm.Item.Id);
                    instance.Name = vm.Item.Name;
                    db.Genres.Update(instance);
                }
                db.SaveChanges();
            }
            vm.Page = Pages.GetPageViewModelAndItems(genres, page, out var items);
            vm.ListItems = items;
            return vm;
        }
        #endregion

        private static void Delete<T>(ApplicationContext db, DbSet<T> table, int id) where T : class, IModel
        {
            var instance = table.FirstOrDefault(x => x.Id.Equals(id));
            if (instance != null)
            {
                try
                {
                    table.Remove(instance);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    
                }
            }
        }
    }
}
