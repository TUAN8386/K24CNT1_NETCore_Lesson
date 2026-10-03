using DAT_LESSON_14_LAYOUT.Models;
using Microsoft.EntityFrameworkCore;

namespace DAT_LESSON_14_LAYOUT.Data
{
    public class DatAppDbContext : DbContext
    {
        public DatAppDbContext(DbContextOptions<DatAppDbContext> options) : base(options) { }

        public DbSet<DatCategory> DatCategories => Set<DatCategory>();
        public DbSet<DatProduct> DatProducts => Set<DatProduct>();
        public DbSet<DatBanner> DatBanners => Set<DatBanner>();
        public DbSet<DatBlog> DatBlogs => Set<DatBlog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // HasSentinel: để EF luôn gửi giá trị người dùng chọn (kể cả 0),
            // DB vẫn có DEFAULT đúng yêu cầu đề bài khi insert bằng SQL.

            // ===== CATEGORY =====
            modelBuilder.Entity<DatCategory>(e =>
            {
                e.HasIndex(x => x.Name).IsUnique();                 // không trùng
                e.Property(x => x.Status).HasDefaultValue((byte)1).HasSentinel(byte.MaxValue);  // mặc định 1
                e.Property(x => x.CreatedDate).HasDefaultValueSql("CAST(GETDATE() AS date)");
            });

            // ===== PRODUCT =====
            modelBuilder.Entity<DatProduct>(e =>
            {
                e.HasIndex(x => x.Name).IsUnique();
                e.Property(x => x.Price).IsRequired();
                e.Property(x => x.SalePrice).HasDefaultValue(0d).HasSentinel(-1d);
                e.Property(x => x.Status).HasDefaultValue((byte)1).HasSentinel(byte.MaxValue);
                e.Property(x => x.CategoryId).IsRequired();
                e.Property(x => x.CreatedDate).HasDefaultValueSql("CAST(GETDATE() AS date)");

                e.HasOne(x => x.Category)
                 .WithMany(c => c.Products)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);               // không xóa danh mục đang có sản phẩm
            });

            // ===== BANNER =====
            modelBuilder.Entity<DatBanner>(e =>
            {
                e.HasIndex(x => x.Name).IsUnique();
                e.Property(x => x.Status).HasDefaultValue((byte)1).HasSentinel(byte.MaxValue);
                e.Property(x => x.Priority).HasDefaultValue(0).HasSentinel(int.MinValue);
            });

            // ===== BLOG =====
            modelBuilder.Entity<DatBlog>(e =>
            {
                e.HasIndex(x => x.Name).IsUnique();
                e.Property(x => x.Status).HasDefaultValue((byte)1).HasSentinel(byte.MaxValue);
                e.Property(x => x.CreatedDate).HasDefaultValueSql("CAST(GETDATE() AS date)");
            });
        }
    }
}
