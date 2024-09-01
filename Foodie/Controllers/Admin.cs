using Foodie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Foodie.Controllers
{
    public class AdminController : Controller
    {
        private readonly FoodieDBContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        

        public AdminController(FoodieDBContext context, IPasswordHasher<User> PasswordHasher)
        {
            _context = context;
            _passwordHasher = PasswordHasher;


        }

        [HttpGet]
        public IActionResult AddProduct()
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

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductName = model.ProductName,
                    Price = model.Price,
                    Time = model.PreparationTime
                };

                if (model.Image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        product.ImageData = memoryStream.ToArray();
                    }
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewMenu");
            }
            return View(model);
        }

        public IActionResult ViewMenu()
        {
            var adminuser = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(adminuser))
            {
                return RedirectToAction("AdminLogin", "Auth");
            }
            else
            {
                var productData = _context.Products.ToList();
                return View(productData);
            }
            
        }

        [HttpGet]
        public IActionResult UpdateMenu(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                PreparationTime = product.Time
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenu(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == model.Id);
                if (product == null)
                {
                    return NotFound();
                }

                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.Time = model.PreparationTime;

                if (model.Image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.Image.CopyToAsync(memoryStream);
                        product.ImageData = memoryStream.ToArray();
                    }
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewMenu));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewMenu));
        }

        [HttpGet]
        public IActionResult AddUser()
        {

            var adminuser = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(adminuser))
            {
                return RedirectToAction("AdminLogin", "Auth");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);

                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                    return View(model);
                }
                model.Password = _passwordHasher.HashPassword(model, model.Password);

                _context.Users.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewUser");
            }
            return View(model);
        }


        public IActionResult ViewUser()
        {
            var adminuser = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(adminuser))
            {
                return RedirectToAction("AdminLogin", "Auth");
            }
            else
            {
                var userData = _context.Users.ToList();
                return View(userData);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewUser));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(model.Id);

                if (user == null)
                {
                    return NotFound();
                }

                user.FName = model.FName;
                user.LName = model.LName;
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.Email = model.Email;
                user.Contact = model.Contact;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewUser));
            }
            return View(model);
        }
        
    }
}
