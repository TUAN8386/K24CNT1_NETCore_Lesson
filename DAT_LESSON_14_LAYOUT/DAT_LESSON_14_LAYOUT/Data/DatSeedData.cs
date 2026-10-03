using DAT_LESSON_14_LAYOUT.Models;

namespace DAT_LESSON_14_LAYOUT.Data
{
    // Dữ liệu mẫu để có sẵn cái hiển thị khi chạy lần đầu
    public static class DatSeedData
    {
        public static void Seed(DatAppDbContext db)
        {
            if (!db.DatCategories.Any())
            {
                db.DatCategories.AddRange(
                    new DatCategory { Name = "Điện thoại", Description = "Các dòng điện thoại thông minh" },
                    new DatCategory { Name = "Laptop", Description = "Máy tính xách tay" },
                    new DatCategory { Name = "Phụ kiện", Description = "Tai nghe, sạc, ốp lưng...", Status = 0 }
                );
                db.SaveChanges();
            }

            if (!db.DatProducts.Any())
            {
                var phone = db.DatCategories.First(c => c.Name == "Điện thoại");
                var laptop = db.DatCategories.First(c => c.Name == "Laptop");
                db.DatProducts.AddRange(
                    new DatProduct { Name = "iPhone 17", Price = 25990000, SalePrice = 24490000, CategoryId = phone.Id },
                    new DatProduct { Name = "Galaxy S26", Price = 22990000, CategoryId = phone.Id },
                    new DatProduct { Name = "ThinkPad X1 Carbon", Price = 38990000, SalePrice = 35990000, CategoryId = laptop.Id }
                );
            }

            if (!db.DatBanners.Any())
            {
                db.DatBanners.AddRange(
                    new DatBanner { Name = "Banner khai trương", Priority = 1, Description = "Giảm giá 50% tuần đầu" },
                    new DatBanner { Name = "Banner mùa thu", Priority = 2 }
                );
            }

            if (!db.DatBlogs.Any())
            {
                db.DatBlogs.AddRange(
                    new DatBlog { Name = "Tìm hiểu Layout trong ASP.NET Core", Description = "_Layout, _ViewImports và _ViewStart" },
                    new DatBlog { Name = "Area là gì?", Description = "Tách khu vực quản trị khỏi trang người dùng" }
                );
            }

            db.SaveChanges();
        }
    }
}
