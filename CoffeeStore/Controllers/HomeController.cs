using CoffeeStore.Models;
using CoffeeStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeStore.Controllers
{
    public class HomeController : KeepLoginController
    {
        private readonly ICoffeeRepository _repository;

        public HomeController(ICoffeeRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(string category = null)
        {
            // Get phone number from session and pass it to ViewBag
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");

            // Get distinct categories from repository
            var categories = _repository.Products.Select(p => p.Category).Distinct().ToList();

            // Filter products based on category
            var products = string.IsNullOrEmpty(category) ?
                _repository.Products.ToList() :
                _repository.Products.Where(p => p.Category == category).ToList();

            // Create ViewModel
            var viewModel = new ProductsViewModel
            {
                Categories = categories,
                Products = products,
                CurrentCategory = category
            };

            ViewBag.Category = category;
            return View(viewModel);
        }
    }
}
