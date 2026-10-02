using DAT_Layout_LESSON_13.Models;
using Microsoft.AspNetCore.Mvc;

namespace DAT_Layout_LESSON_13.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DatProductManageController : Controller
    {
        // GET: /Admin/DatProductManage/DatIndex – bảng quản lý sản phẩm (dữ liệu mẫu)
        public IActionResult DatIndex()
        {
            return View(DatProductData.DatProducts);
        }
    }
}
