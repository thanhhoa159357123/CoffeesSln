using CoffeeStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeStore.Components
{
    public class CartSummary : ViewComponent
    {
        private Cart cart;
        public CartSummary(Cart cartService)
        {
            cart = cartService;
        }
        public IViewComponentResult Invoke()
        {
            return View(cart);
        }

    }
}
