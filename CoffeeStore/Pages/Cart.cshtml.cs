using CoffeeStore.Infrastructure;
using CoffeeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeStore.Pages
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly ICoffeeRepository _repository;

        public CartModel(ICoffeeRepository repository)
        {
            _repository = repository;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            Cart = SessionCart.GetCart(HttpContext.RequestServices);
        }

        public IActionResult OnPostRemove(string productId, string returnUrl)
        {
            Cart = SessionCart.GetCart(HttpContext.RequestServices);

            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                Cart.RemoveLine(product);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPost(string productId, string returnUrl)
        {
            Cart = SessionCart.GetCart(HttpContext.RequestServices);

            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                Cart.AddItem(product, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
