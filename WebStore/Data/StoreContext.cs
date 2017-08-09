using WebStore.Models;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Catagory> Catagories { get; set; }

        // (Optional) Removes Plural naming in the Database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<StoreItem>().ToTable("StoreItem");
            //modelBuilder.Entity<Company>().ToTable("Company");

            var productImages = modelBuilder.Entity<ProductImage>();
            productImages.HasKey(storeItemImage => new { storeItemImage.ProductId, storeItemImage.ImageId });

            productImages.HasOne(storeItemImage => storeItemImage.Product)
                .WithMany(storeItem => storeItem.ProductImages)
                .HasForeignKey(storeItemImage => storeItemImage.ProductId);

            productImages.HasOne(storeItemImage => storeItemImage.Image)
                .WithMany()
                .HasForeignKey(storeItemImage => storeItemImage.ImageId);

            return;
        }
    }
}
