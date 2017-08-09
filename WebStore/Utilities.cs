using System.Collections.Generic;
using System.Linq;
using WebStore.Models;

public class Utilities
{
    public static List<int> CalculateQuantities(List<Product> products, IList<int> productIds)
    {
        var idGroups = productIds.GroupBy(n => n);
        //var idGroups = products.GroupBy(product => product.Id);

        int[] quantities = new int[idGroups.Count()];
        for (int index = 0; index < idGroups.Count(); index++)
        {
            quantities[index] = idGroups.ElementAtOrDefault(index).Count();
        }

        return quantities.ToList();
    }
}
