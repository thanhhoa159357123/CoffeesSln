using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoffeeStore.Controllers
{
    public class KeepLoginController : Controller, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Lấy số điện thoại từ session và đặt vào ViewBag
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
