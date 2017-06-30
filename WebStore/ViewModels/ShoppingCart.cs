using System.Collections.Generic;

namespace WebStore.ViewModels
{
    public class ShoppingCart
    {
        public IList<int> ProductIds { get; } = new List<int>();
    }
}