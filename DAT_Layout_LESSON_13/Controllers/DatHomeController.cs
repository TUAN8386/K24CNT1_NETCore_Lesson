using Microsoft.AspNetCore.Mvc;

namespace DAT_Layout_LESSON_13.Controllers
{
    // Controller dùng Layout mặc định (_DatLayout.cshtml khai báo trong _ViewStart)
    public class DatHomeController : Controller
    {
        // GET: /DatHome/DatIndex – minh hoạ RenderBody + RenderSection
        public IActionResult DatIndex()
        {
            return View();
        }

        public IActionResult DatError()
        {
            return View();
        }
    }
}
