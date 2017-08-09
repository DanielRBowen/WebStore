using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.ViewModels
{
    public class CartViewModel
    {
        public List<Product> Products { get; set; }

        public List<int> Quantities { get; set; }
    }
}
