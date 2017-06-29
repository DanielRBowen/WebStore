using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.ViewModels
{
    public class BrowseProductsViewModel
    {
        public IList<Catagory> Categories { get; set; }

        public PaginatedList<StoreItem> Products { get; set; }

        public int? CategoryId { get; set; }

        public string SearchQuery { get; set; }
    }
}