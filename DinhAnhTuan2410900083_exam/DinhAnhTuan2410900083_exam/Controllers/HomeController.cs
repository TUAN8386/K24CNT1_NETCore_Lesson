using DinhAnhTuan2410900083_exam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DinhAnhTuan2410900083_exam.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DATAbout()
        {
            ViewBag.MaSV = "2410900083";
            ViewBag.HoTen = "Định Anh Tuấn";
            ViewBag.Lop = "CNTT";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
