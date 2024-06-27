using CoffeeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoffeeStore.Components
{
    public class NavigationMenu : ViewComponent
    {
        private ICoffeeRepository repository;
        public NavigationMenu(ICoffeeRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            var categories = repository.Products
                                .Select(p => p.Category)
                                .Distinct()
                                .OrderBy(c => c);

            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(categories);
        }


    }
}
