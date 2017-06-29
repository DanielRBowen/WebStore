using WebStore.Models;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<StoreItemImage> StoreItemImages { get; set; }

        public DbSet<Catagory> Catagories { get; set; }

        // (Optional) Removes Plural naming in the Database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<StoreItem>().ToTable("StoreItem");
            //modelBuilder.Entity<Company>().ToTable("Company");

            var storeItemImages = modelBuilder.Entity<StoreItemImage>();
            storeItemImages.HasKey(storeItemImage => new { storeItemImage.StoreItemId, storeItemImage.ImageId });

            storeItemImages.HasOne(storeItemImage => storeItemImage.StoreItem)
                .WithMany(storeItem => storeItem.StoreItemImages)
                .HasForeignKey(storeItemImage => storeItemImage.StoreItemId);

            storeItemImages.HasOne(storeItemImage => storeItemImage.Image)
                .WithMany()
                .HasForeignKey(storeItemImage => storeItemImage.ImageId);

            return;
        }
    }
}
