using System.IO;
using System.Linq;
using System.Reflection;
using WebStore.Models;

namespace WebStore.Data
{
    public class DbInitializer
    {
        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();

            // Look for any store items.
            if (context.Products.Any())
            {
                return; // DB has been seeded
            }

            var companies = new Company[]
            {
                new Company { Name = "RAM Company"},
                new Company { Name = "CPU Company"}
            };
            foreach (Company company in companies)
            {
                context.Companies.Add(company);
            }
            context.SaveChanges();

            var ramCatagory = new Catagory { Name = "RAM" };
            context.Catagories.Add(ramCatagory);

            var cpuCatagory = new Catagory { Name = "CPU" };
            context.Catagories.Add(cpuCatagory);

            var storeItems = context.Products;

            var storeItem1 = new Product { Name = "RAM1", Catagory = ramCatagory, Price = 1, Description = "This is RAM 1", Company = companies[0] };
            storeItems.Add(storeItem1);

            var storeItem2 = new Product { Name = "RAM2", Catagory = ramCatagory, Price = 1, Description = "This is RAM 2", Company = companies[0] };
            storeItems.Add(storeItem2);

            var storeItem3 = new Product { Name = "RAM3", Catagory = ramCatagory, Price = 1, Description = "This is RAM 3", Company = companies[0] };
            storeItems.Add(storeItem3);

            var storeItem4 = new Product { Name = "CPU1", Catagory = cpuCatagory, Price = 2, Description = "This is CPU 1", Company = companies[1] };
            storeItems.Add(storeItem4);

            var storeItem5 = new Product { Name = "CPU2", Catagory = cpuCatagory, Price = 2, Description = "This is CPU 2", Company = companies[1] };
            storeItems.Add(storeItem5);

            var storeItem6 = new Product { Name = "CPU3", Catagory = cpuCatagory, Price = 2, Description = "This is CPU 3", Company = companies[1] };
            storeItems.Add(storeItem6);

            var currentFolderPath = Assembly.GetEntryAssembly().Location;

            var ramImage = new Image
            {
                FileName = "RedRam.jpg",
                MediaType = "image/jpeg",
                Content = File.ReadAllBytes(Path.Combine(currentFolderPath, "../../../../SampleImages/20-158-024-04.jpg"))
            };
            context.Images.Add(ramImage);

            context.ProductImages.Add(new ProductImage { Product = storeItem1, Image = ramImage });

            var cpuImage = new Image
            {
                FileName = "Corei7.jpg",
                MediaType = "image/jpeg",
                Content = File.ReadAllBytes(Path.Combine(currentFolderPath, "../../../../SampleImages/19-117-726-Z01.jpg"))
            };
            context.Images.Add(cpuImage);

            context.ProductImages.Add(new ProductImage { Product = storeItem2, Image = cpuImage });

            context.SaveChanges();
        }
    }
}
