using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebStore.API;
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

        public async Task<IActionResult> Checkout()
        {
            var viewModel = await CreateCheckoutViewModel();

            if (viewModel == null)
            {
                TempData["SuccessMessage"] = "You must register to check out or save.";
                return RedirectToAction(nameof(CartController.Cart), "Cart");
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Confirmation()
        {
            var viewModel = await CreateCheckoutViewModel();

            return View(viewModel);
        }

        public async Task<CheckoutViewModel> CreateCheckoutViewModel()
        {
            var user = HttpContext.Session.Get<User>("User");

            if (user == null)
            {
                return null;
            }

            var shoppingCart = HttpContext.Session.Get<ShoppingCart>("ShoppingCart");

            var productIds = shoppingCart.ProductIds.ToList();
            var productsTable = new List<StoreItem>();
            productsTable = await _context.StoreItems.ToListAsync();

            foreach (var productId in productIds)
            {
                user.Cart.Add(productsTable.FirstOrDefault(product => product.Id == productId));
            }

            var url = Url.Action(nameof(TaxesController.GetTaxRate), "Taxes", new { zipcode = 84037 }, Request.Scheme);
            var webRequest = WebRequest.Create(new Uri(url, UriKind.Absolute));
            decimal taxRate;

            using (var response = await webRequest.GetResponseAsync())
            using (var inputStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(inputStream))
            {
                var jsonTextReader = new JsonTextReader(streamReader);
                var nullableTaxRate = jsonTextReader.ReadAsDecimal();
                taxRate = nullableTaxRate.GetValueOrDefault();
            }

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
