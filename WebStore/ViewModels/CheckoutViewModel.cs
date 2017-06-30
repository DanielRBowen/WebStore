using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.ViewModels
{
    public class CheckoutViewModel
    {
        public List<StoreItem> Products { get; set; }

        public List<int> Quantities { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }
    }
}
