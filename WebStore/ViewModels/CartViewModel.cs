using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.ViewModels
{
    public class CartViewModel
    {
        public List<StoreItem> Products { get; set; }

        public List<int> Quantities { get; set; }
    }
}
