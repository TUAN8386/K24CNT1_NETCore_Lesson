using DatNetCoreLAB6_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Data
{
    // CSDL DatNetCoreCRUD: DatCategory, DatProduct, DatBanner
    public class DatAppDbContext : DbContext
    {
        public DatAppDbContext(DbContextOptions<DatAppDbContext> options) : base(options) { }

        public DbSet<DatCategory> DatCategories => Set<DatCategory>();
        public DbSet<DatProduct> DatProducts => Set<DatProduct>();
        public DbSet<DatBanner> DatBanners => Set<DatBanner>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1 danh mục - n sản phẩm, không cho xóa danh mục khi còn sản phẩm
            modelBuilder.Entity<DatProduct>()
                .HasOne(datP => datP.Category)
                .WithMany(datC => datC.Products)
                .HasForeignKey(datP => datP.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
