using Microsoft.AspNetCore.Mvc;

namespace DAT_Lesson08_Model.Controllers
{
    public class DATHomeController : Controller
    {
        // GET: /DATHome/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /DATHome/DATAbout
        public IActionResult DATAbout()
        {
            ViewData["Title"] = "Thông tin sinh viên";
            return View();
        }
    }
}