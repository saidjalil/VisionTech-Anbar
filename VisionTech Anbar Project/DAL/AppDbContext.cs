using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Entities.Categories;
using Image = VisionTech_Anbar_Project.Entities.Image;
using Type = VisionTech_Anbar_Project.Entities.Categories.Type;

namespace VisionTech_Anbar_Project.DAL
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 4. Configure Connection String
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VisionTechAnbar;Trusted_Connection=True;MultipleActiveResultSets=True;");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<PackageProduct> PackageProducts { get; set; }
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Image> Images { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackageProduct>()
        .HasKey(pp => new { pp.PackageId, pp.ProductId });

            modelBuilder.Entity<PackageProduct>()
                .Property(pp => pp.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PackageProduct>()
                .HasOne(pp => pp.Package)
                .WithMany(p => p.PackageProducts)
                .HasForeignKey(pp => pp.PackageId);

            modelBuilder.Entity<PackageProduct>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.PackageProducts)
                .HasForeignKey(pp => pp.ProductId);

            // Configure the many-to-many relationship between Package and Product through PackageProduct
            modelBuilder.Entity<PackageProduct>()
                .HasOne(pp => pp.Package)
                .WithMany(p => p.PackageProducts)
                .HasForeignKey(pp => pp.PackageId)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: Specify delete behavior

            modelBuilder.Entity<PackageProduct>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.PackageProducts)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> Category
            modelBuilder.Entity<Product>()
                .HasOne<Category>(p => p.Category) // Product has one Category
                .WithMany() // Category doesn't explicitly track Products
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Delete Products if Category is deleted


            // Category -> SubCategories (Self-referencing)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent) // A Category has one parent
                .WithMany(c => c.SubCategories) // A Category has many subcategories
                .HasForeignKey(c => c.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); // Prevent recursive deletes

            // Vendor -> Package
            modelBuilder.Entity<Package>()
                .HasOne<Vendor>(pkg => pkg.Vendor) // Package has one Vendor
                .WithMany(v => v.Packages)        // Vendor has many Packages
                .HasForeignKey(pkg => pkg.VendorId);

            // Warehouse -> Package
            modelBuilder.Entity<Package>()
                .HasOne<Warehouse>(pkg => pkg.Warehouse) // Package has one Warehouse
                .WithMany(w => w.Packages)              // Warehouse has many Packages
                .HasForeignKey(pkg => pkg.WarehouseId);


            modelBuilder.Entity<Barcode>()
                .HasOne(b => b.Product) // Each Barcode has one Product
                .WithMany(p => p.Barcodes) // Each Product can have many Barcodes
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if the Product is deleted

            // Unique constraint on BarCode
            modelBuilder.Entity<Barcode>()
                .HasIndex(b => b.BarCode)
                .IsUnique();
        }

    }
}
