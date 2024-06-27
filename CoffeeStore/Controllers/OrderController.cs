using Microsoft.AspNetCore.Mvc;
using CoffeeStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CoffeeStore.Controllers
{
    [Authorize]
    public class OrderController : KeepLoginController
    {
        private readonly IOrderRepository _repository;
        private readonly Cart _cart;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderRepository repoService, Cart cartService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repoService;
            _cart = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Order()
        {
            // Retrieve user information from HttpContext.User
            var user = _httpContextAccessor.HttpContext.User;
            var customer = new Customer
            {
                FirstName = user.FindFirstValue(ClaimTypes.GivenName), // Adjust based on your claims
                LastName = user.FindFirstValue(ClaimTypes.Surname) // Adjust based on your claims
            };

            var order = new Order
            {
                Phone = user.FindFirstValue("phone_number"), // Adjust "phone_number" to match your claim name
                Customer = customer, // Set the customer object
                OrderLines = _cart.Lines.ToArray()
            };

            return View(order);
        }

        [HttpPost]
        public IActionResult Order(Order order)
        {
            if (_cart.Lines.Count == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.OrderLines = _cart.Lines.ToArray();
                _repository.SaveOrder(order);
                _cart.Clear();
                return RedirectToPage("/Completed", new { orderId = order.OrderID });
            }
            else
            {
                return View(order);
            }
        }
    }
}
