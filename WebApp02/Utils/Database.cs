using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApp02.Models;
using WebApp02.ViewModel;

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
        private static BaseInsertViewModel<PublishingHouse> PublishingHouse(BaseInsertViewModel<PublishingHouse> vm, ApplicationContext db, ModelStateDictionary modelState, bool insert, int page = 1)
        {
            var pubHouses = db.PublishingHouses.AsQueryable();

            vm.Item ??= new();
            vm.Item.Name = vm.Item.Name.Trim();
            bool isExist = pubHouses.Any(x => x.Name == vm.Item.Name);
            if (isExist)
                modelState.AddModelError("PublishingHouse", "Такой издатель уже существует!");

            if (modelState.IsValid)
            {
                if (insert)
                    CRUD.Insert(db.PublishingHouses, vm.Item);
                else
                    CRUD.Update(db.PublishingHouses, vm.Item);

                db.SaveChanges();
                vm = new()
                {
                    Item = new()
                };
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
        private static BaseInsertViewModel<Autor> Autor(BaseInsertViewModel<Autor> vm, ApplicationContext db, ModelStateDictionary modelState, bool insert, int page = 1)
        {
            var autors = db.Autors.AsQueryable();

            vm.Item ??= new();
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

            if (modelState.IsValid)
            {
                if (insert)
                    CRUD.Insert(db.Autors, vm.Item);
                else
                    CRUD.Update(db.Autors, vm.Item);

                db.SaveChanges();
                vm = new()
                {
                    Item = new()
                };
            }
            vm.Page = Pages.GetPageViewModelAndItems(autors, page, out var items);
            vm.ListItems = items;
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
        private static BaseInsertViewModel<Genre> Genre(BaseInsertViewModel<Genre> vm, ApplicationContext db, ModelStateDictionary modelState, bool insert, int page = 1)
        {
            var genres = db.Genres.AsQueryable();

            vm.Item ??= new();
            vm.Item.Name = vm.Item.Name?.Trim();
            bool isExist = genres.Any(x => x.Name == vm.Item.Name);
            if (isExist)
                modelState.AddModelError("Genre", "Такой жанр уже существует!");

            if (modelState.IsValid)
            {
                if (insert)
                    CRUD.Insert(db.Genres, vm.Item);
                else
                    CRUD.Update(db.Genres, vm.Item);
                db.SaveChanges();
                vm = new()
                {
                    Item = new()
                };
            }
            vm.Page = Pages.GetPageViewModelAndItems(genres, page, out var items);
            vm.ListItems = items;
            return vm;
        }
        #endregion

    }
}
