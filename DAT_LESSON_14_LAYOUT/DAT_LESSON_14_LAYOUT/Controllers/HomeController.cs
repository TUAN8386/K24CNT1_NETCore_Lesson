using System.Diagnostics;
using DAT_LESSON_14_LAYOUT.Data;
using DAT_LESSON_14_LAYOUT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAT_LESSON_14_LAYOUT.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatAppDbContext _db;

        public HomeController(DatAppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.DatBanner = await _db.DatBanners
                .Where(b => b.Status == 1).OrderBy(b => b.Priority).FirstOrDefaultAsync();

            ViewBag.DatProducts = await _db.DatProducts
                .Where(p => p.Status == 1).OrderByDescending(p => p.Id).Take(3).ToListAsync();

            ViewBag.DatBlogs = await _db.DatBlogs
                .Where(b => b.Status == 1).OrderByDescending(b => b.Id).Take(6).ToListAsync();

            return View();
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        // Demo layout gốc _Layout (Bài 1)
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
