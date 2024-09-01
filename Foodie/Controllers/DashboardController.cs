using Microsoft.AspNetCore.Mvc;

namespace Foodie.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var adminuser = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(adminuser))
            {
                // Redirect to login page if customer is not logged in
                return RedirectToAction("AdminLogin", "Auth");
            }
            else
            {
                return View();
            }
        }
    }
}
