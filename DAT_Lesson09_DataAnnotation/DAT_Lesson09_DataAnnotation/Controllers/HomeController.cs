using DAT_Lesson09_DataAnnotation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DAT_Lesson09_DataAnnotation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Tự động chuyển hướng người dùng sang trang Account/Index
            return RedirectToAction("Index", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
