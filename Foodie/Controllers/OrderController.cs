using Foodie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Foodie.Controllers
{
    public class OrderController : Controller
    {
        private readonly FoodieDBContext _context;

        public OrderController(FoodieDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ViewOrder()
        {
            var adminuser = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(adminuser))
            {
                // Redirect to login page if customer is not logged in
                return RedirectToAction("AdminLogin", "Auth");
            }
            else
            {
                var orderData = await _context.Orders.ToListAsync();
                return View(orderData);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Completed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatus = 1; // Set status to 'Delivered'
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ViewOrder));
        }

        [HttpPost]
        public async Task<IActionResult> Pending(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatus = 0; // Set status to 'Pending'
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ViewOrder));
        }
    }
}
