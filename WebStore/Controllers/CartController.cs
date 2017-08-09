using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebStore.Data;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private StoreContext StoreContext { get; }

        public CartController(StoreContext storeContext)
        {
            StoreContext = storeContext ?? throw new ArgumentNullException(nameof(storeContext));
        }

        public IActionResult Cart()
        {
            var shoppingCart = HttpContext.Session.Get<ShoppingCart>("ShoppingCart");

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
            }

            var productIds = shoppingCart.ProductIds;

            var productsQuery =
                from storeItem in StoreContext.Products
                    .AsNoTracking()
                    .Include(storeItem0 => storeItem0.ProductImages)
                select storeItem;

            productsQuery = productsQuery.Where(product => productIds.Contains(product.Id));
            productsQuery = productsQuery.OrderBy(product => product.Id);

            //var idGroups = productIds.GroupBy(n => n);

            //int[] quantities = new int[idGroups.Count()];
            //for (int index = 0; index < idGroups.Count(); index++)
            //{
            //    quantities[index] = idGroups.ElementAtOrDefault(index).Count();
            //}

            var products = productsQuery.ToList();
            var viewModel = new CartViewModel
            {
                Products = productsQuery.ToList(),
                Quantities = Utilities.CalculateQuantities(products, productIds)
            };

            return View(viewModel);
        }

        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var shoppingCart = HttpContext.Session.Get<ShoppingCart>("ShoppingCart");

            var productIds = shoppingCart.ProductIds;

            // Remove all of those ids
            while (productIds.Contains(id))
            {
                productIds.Remove(id);
            }

            // Add the ids of the new quantity
            for (var index = 0; index < quantity; ++index)
            {
                productIds.Add(id);
            }
            productIds.OrderByDescending(x => x);

            HttpContext.Session.Set("ShoppingCart", shoppingCart);
            TempData["SuccessMessage"] = "Successfully updated the quantity.";

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult RemoveFromCart(int id)
        {
            var shoppingCart = HttpContext.Session.Get<ShoppingCart>("ShoppingCart");

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
            }

            var productIds = shoppingCart.ProductIds;

            // Remove all of those ids
            while (productIds.Contains(id))
            {
                productIds.Remove(id);
            }

            HttpContext.Session.Set("ShoppingCart", shoppingCart);
            TempData["SuccessMessage"] = "Successfully removed from shopping cart.";

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult SaveCart()
        {
            var user = HttpContext.Session.Get<User>("User");

            if (user == null)
            {
                TempData["SuccessMessage"] = "You must register to check out or save.";
                return RedirectToAction(nameof(CartController.Cart), "Cart");
            }

            var shoppingCart = HttpContext.Session.Get<ShoppingCart>("ShoppingCart");

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
            }

            HttpContext.Session.Set("ShoppingCart", shoppingCart);
            TempData["SuccessMessage"] = "Successfully saved the shopping cart.";

            return RedirectToAction(nameof(Cart));
        }
    }
}