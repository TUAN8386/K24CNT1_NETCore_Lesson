using Microsoft.AspNetCore.Mvc;

namespace DAT_Layout_LESSON_13.Areas.Admin.Controllers
{
    // Controller thuộc Area Admin – dùng Layout _DatLayoutAdmin.cshtml
    [Area("Admin")]
    public class DatDashboardController : Controller
    {
        // GET: /Admin/DatDashboard/DatIndex
        public IActionResult DatIndex()
        {
            return View();
        }
    }
}
