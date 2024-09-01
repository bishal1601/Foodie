using Foodie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Foodie.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodieDBContext _context;

        public HomeController(FoodieDBContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var productData = await _context.Products.ToListAsync();
            return View(productData);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var customerIdStr = HttpContext.Session.GetString("CustomerId");

            if (string.IsNullOrEmpty(customerIdStr))
            {
                // Return JSON response for the front-end to handle redirection
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Auth") });
            }
            var customerId = Convert.ToInt32(customerIdStr);

            var product = _context.Products.Find(productId);
            if (product == null)
            {
                // Handle case where product is not found
                return Json(new { success = false, message = "Product not found." });
            }

            var existingCartItem = _context.Carts.FirstOrDefault(c => c.CustomerId == customerId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Qty++;
            }
            else
            {
                var newCartItem = new Cart
                {
                    CustomerId = customerId,
                    ProductId = productId,
                    ProductName = product.ProductName,
                    Qty = 1,
                    Price = product.Price
                };
                _context.Carts.Add(newCartItem);
            }

            _context.SaveChanges();
            var cartItemCount = _context.Carts.Count(c => c.CustomerId == customerId);
            HttpContext.Session.SetString("CartCount", cartItemCount.ToString());

            return Json(new { success = true, cartItemCount });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCart(int cartId)
        {
            var cartItem = await _context.Carts.FindAsync(cartId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ViewCarts");
        }

        public async Task<IActionResult> ViewCarts()
        {
            var cartData = await _context.Carts.ToListAsync();
            return View(cartData);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var customerIdStr = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerIdStr))
            {
                // Redirect to login page if customer is not logged in
                return RedirectToAction("Login", "Auth");
            }
            var customerId = Convert.ToInt32(customerIdStr);

            var cartItems = await _context.Carts.Where(c => c.CustomerId == customerId).ToListAsync();
            if (!cartItems.Any())
            {
                return RedirectToAction("ViewCarts"); // No items in cart to checkout
            }

            // Retrieve customer information from session
            var customerFName = HttpContext.Session.GetString("CustomerFname");
            var customerLName = HttpContext.Session.GetString("CustomerLname");
            var customerAddress = HttpContext.Session.GetString("CustomerAddress");
            var customerContact = HttpContext.Session.GetString("CustomerContact");

            foreach (var cartItem in cartItems)
            {
                var order = new Order
                {
                    CFName = customerFName,
                    CLname = customerLName,
                    Address = customerAddress,
                    Contact = customerContact, // Make sure your Order model has a Contact property
                    ProductName = cartItem.ProductName,
                    Qty = cartItem.Qty,
                    price = cartItem.Price,
                    OrderStatus = 0 // Initial order status
                };
                _context.Orders.Add(order);
            }

            // Remove cart items after they have been transferred to the order table
            _context.Carts.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewCarts"); // Redirect back to the cart view
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
