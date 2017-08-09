using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class ProductsController : Controller
    {
        private StoreContext StoreContext { get; }

        public ProductsController(StoreContext storeContext)
        {
            StoreContext = storeContext ?? throw new ArgumentNullException(nameof(storeContext));
        }

        /// <summary>
        /// Visitors can "Search" by for a product by Product Keyword (in either the name or description)
        /// Product result pages are paged and displayed in such a way (beyond the stand EF grid) to show Thumbnail Images, Descriptions, pricing etc.
        /// Visitors can browse Products by Category
        /// Visitors can view a single product in more "detail" (with full sized images), and add it to their "cart" (including product qty)
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="searchQuery"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? categoryId, string searchQuery, int? page)
        {
            var catagories = StoreContext.Catagories.AsNoTracking().ToList();

            var productsQuery =
                from storeItem in StoreContext.Products
                    .AsNoTracking()
                    .Include(storeItem0 => storeItem0.ProductImages)
                select storeItem;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                productsQuery = productsQuery.Where(storeItem => storeItem.Name.Contains(searchQuery)
                    || storeItem.Description.Contains(searchQuery));
            }

            if (categoryId != null)
            {
                var categoryIdValue = categoryId.GetValueOrDefault();
                productsQuery = productsQuery.Where(product => product.CatagoryId == categoryIdValue);
            }

            productsQuery = productsQuery.OrderBy(product => product.Name);

            const int pageSize = 4;
            var products = await PaginatedList<Product>.CreateAsync(productsQuery, page ?? 1, pageSize);

            var viewModel = new BrowseProductsViewModel
            {
                Categories = catagories,
                Products = products,
                CategoryId = categoryId,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }

        public async Task<IActionResult> View(int id)
        {
            var productQuery =
                from product0 in StoreContext.Products.AsNoTracking()
                    .Include(product0 => product0.ProductImages)
                where product0.Id == id
                select product0;

            var product = await productQuery.FirstOrDefaultAsync();

            if (productQuery == null)
            {
                return NotFound();
            }

            var viewModel = new ViewProductViewModel
            {
                Product = product
            };

            return View(viewModel);
        }

        public IActionResult AddToCart(int id, int quantity)
        {
            var shoppingCart = HttpContext.Session.Get<ShoppingCart>("ShoppingCart");

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
            }

            var productIds = shoppingCart.ProductIds;

            for (var index = 0; index < quantity; ++index)
            {
                productIds.Add(id);
            }

            HttpContext.Session.Set("ShoppingCart", shoppingCart);
            TempData["SuccessMessage"] = "Successfully added to shopping cart.";

            return RedirectToAction(nameof(View), new { id = id });
        }
    }
}