using Foodie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Foodie.Controllers
{
    public class AuthController : Controller
    {
        private readonly FoodieDBContext _context;
        private readonly IPasswordHasher<Customer> _customerPasswordHasher;
        private readonly IPasswordHasher<User> _userPasswordHasher;

        public AuthController(FoodieDBContext context, IPasswordHasher<Customer> customerPasswordHasher, IPasswordHasher<User> userPasswordHasher)
        {
            _context = context;
            _customerPasswordHasher = customerPasswordHasher;
            _userPasswordHasher = userPasswordHasher;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(Customer customer)
        {
            var myCustomer = _context.Customers.FirstOrDefault(x => x.UserName == customer.UserName);

            if (myCustomer != null)
            {
                var verificationResult = _customerPasswordHasher.VerifyHashedPassword(myCustomer, myCustomer.Password, customer.Password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("CustomerFname", myCustomer.FName);
                    HttpContext.Session.SetString("CustomerLname", myCustomer.LName);
                    HttpContext.Session.SetString("CustomerAddress", myCustomer.Address);
                    HttpContext.Session.SetString("CustomerContact", myCustomer.Contact);
                    HttpContext.Session.SetString("CustomerId", myCustomer.Id.ToString());

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.loginError = "Invalid Username and Password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AdminLogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogIn(User user)
        {
            var myUser = _context.Users.FirstOrDefault(x => x.UserName == user.UserName);

            if (myUser != null)
            {
                var verificationResult = _userPasswordHasher.VerifyHashedPassword(myUser, myUser.Password, user.Password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("Username", myUser.UserName);
                    HttpContext.Session.SetString("UserType", "admin");

                    return Redirect("/Dashboard/Index");
                }
            }

            ViewBag.loginError = "Invalid Username and Password";
            return View();
        }
    }
}
