using Foodie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Foodie.Controllers
{
    public class CustomerController : Controller
    {
        private readonly FoodieDBContext _context;
        private readonly IPasswordHasher<Customer> _passwordHasher;

        public CustomerController(FoodieDBContext context, IPasswordHasher<Customer> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult CustomerRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CustomerRegister(Customer model)
        {
            if (ModelState.IsValid)
            {
                // Check if email, username, or contact already exists
                var existingCustomer = await _context.Customers
                    .Where(c => c.Email == model.Email || c.UserName == model.UserName || c.Contact == model.Contact)
                    .FirstOrDefaultAsync();

                if (existingCustomer != null)
                {
                    if (existingCustomer.Email == model.Email)
                    {
                        ModelState.AddModelError("Email", "Email is already in use.");
                    }
                    if (existingCustomer.UserName == model.UserName)
                    {
                        ModelState.AddModelError("UserName", "Username is already in use.");
                    }
                    if (existingCustomer.Contact == model.Contact)
                    {
                        ModelState.AddModelError("Contact", "Contact number is already in use.");
                    }
                    return View(model);
                }

                // Hash the password before saving
                model.Password = _passwordHasher.HashPassword(model, model.Password);

                _context.Customers.Add(model);
                await _context.SaveChangesAsync();
                return Redirect("/auth/login");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewCustomer()
        {
            var adminuser = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(adminuser))
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var customers = await _context.Customers.ToListAsync();
            return View(customers);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(Customer model)
        {
            if (ModelState.IsValid)
            {
                // Check if email, username, or contact already exists for other customers
                var existingCustomer = await _context.Customers
                    .Where(c => (c.Email == model.Email || c.UserName == model.UserName || c.Contact == model.Contact) && c.Id != model.Id)
                    .FirstOrDefaultAsync();

                if (existingCustomer != null)
                {
                    if (existingCustomer.Email == model.Email)
                    {
                        ModelState.AddModelError("Email", "Email is already in use.");
                    }
                    if (existingCustomer.UserName == model.UserName)
                    {
                        ModelState.AddModelError("UserName", "Username is already in use.");
                    }
                    if (existingCustomer.Contact == model.Contact)
                    {
                        ModelState.AddModelError("Contact", "Contact number is already in use.");
                    }
                    return View(model);
                }

                // Hash the password before updating
                if (!string.IsNullOrEmpty(model.Password))
                {
                    model.Password = _passwordHasher.HashPassword(model, model.Password);
                }

                _context.Customers.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewCustomer");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomerConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ViewCustomer");
        }
    }
}
