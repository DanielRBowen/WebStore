using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.ViewModels
{
    public class ShoppingCart
    {
        public IList<int> ProductIds { get; } = new List<int>();
    }
}