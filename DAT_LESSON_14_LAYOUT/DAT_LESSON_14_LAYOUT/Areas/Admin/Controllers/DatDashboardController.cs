using DAT_LESSON_14_LAYOUT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAT_LESSON_14_LAYOUT.Areas.Admin.Controllers
{
    // Bài 3 - Bước 7: controller mặc định khi truy cập /Admin
    [Area("Admin")]
    public class DatDashboardController : Controller
    {
        private readonly DatAppDbContext _db;

        public DatDashboardController(DatAppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.DatCategoryCount = await _db.DatCategories.CountAsync();
            ViewBag.DatProductCount = await _db.DatProducts.CountAsync();
            ViewBag.DatBannerCount = await _db.DatBanners.CountAsync();
            ViewBag.DatBlogCount = await _db.DatBlogs.CountAsync();
            return View();
        }
    }
}
