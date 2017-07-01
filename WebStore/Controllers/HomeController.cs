using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreContext _context;

        public HomeController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index() => RedirectToAction(nameof(ProductsController.Index), "Products");

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Administration()
        {
            return View();
        }

        public IActionResult Registration()
        {
            var user = HttpContext.Session.Get<User>("User");

            if (user == null)
            {
                user = new User();
            }

            return View(user);
        }

        public async Task<IActionResult> SaveUser(int id, [Bind("Id, Email, Password")] User user)
        {
            //if (user.Email == null || user.Password == null)
            //{
            //    TempData["SuccessMessage"] = "User must have a Email and password";
            //    return RedirectToAction(nameof(Registration));
            //}

            // Update the user in the database if the user exists or add a new user if does not.
            if (ModelState.IsValid)
            {
                if (UserExists(id))
                {
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    HttpContext.Session.Set("User", user);
                    TempData["SuccessMessage"] = "You have logged in.";

                    return RedirectToAction(nameof(Registration));
                }
                else
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Set("User", user);
                    TempData["SuccessMessage"] = "You have Registered and logged in.";

                    return RedirectToAction(nameof(Registration));
                }
            }
            else
            {
                TempData["SuccessMessage"] = "User must have a valid Email and password";
                return RedirectToAction(nameof(Registration));
            }
        }

        public IActionResult Checkout()
        {
            var viewModel = CreateCheckoutViewModel();

            if (viewModel == null)
            {
                TempData["SuccessMessage"] = "You must register to check out or save.";
                return RedirectToAction(nameof(CartController.Cart), "Cart");
            }

            return View(viewModel);
        }

        public IActionResult Confirmation()
        {
            var viewModel = CreateCheckoutViewModel();

            return View(viewModel);
        }

        public CheckoutViewModel CreateCheckoutViewModel()
        {
            var user = HttpContext.Session.Get<User>("User");

            if (user == null)
            {
                return null;
            }

            var shoppingCart = HttpContext.Session.Get<ShoppingCart>("ShoppingCart");

            var productIds = shoppingCart.ProductIds.ToList();
            var productsTable = new List<StoreItem>();
            productsTable = _context.StoreItems.ToList();
            foreach (var productId in productIds)
            {
                user.Cart.Add(productsTable.FirstOrDefault(product => product.Id == productId));
            }

            //string rawTaxRate = Request.Host[];
            decimal taxRate = (decimal)0.1;
            decimal total = 0;

            foreach (var product in productsTable)
            {
                total += product.Price;
            }
            decimal tax = total * taxRate;
            total += tax;

            var products = user.Cart.Distinct().ToList();
            var viewModel = new CheckoutViewModel
            {
                Products = products,
                Quantities = Utilities.CalculateQuantities(products, productIds),
                Tax = tax,
                Total = total
            };

            return viewModel;
        }

        public IActionResult Cart() => RedirectToAction(nameof(CartController.Cart), "Cart");

        public IActionResult Error()
        {
            return View();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
