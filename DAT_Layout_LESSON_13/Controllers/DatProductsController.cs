using DAT_Layout_LESSON_13.Models;
using Microsoft.AspNetCore.Mvc;

namespace DAT_Layout_LESSON_13.Controllers
{
    // Controller trang Customer – dùng Custom Layout _DatLayoutHome.cshtml
    public class DatProductsController : Controller
    {
        private static readonly List<DatProduct> datProducts = DatProductData.DatProducts;

        // GET: /DatProducts/DatIndex  -> Danh sách sản phẩm
        public IActionResult DatIndex()
        {
            return View(datProducts);
        }

        // GET: /DatProducts/DatSearch?datKeyword=...  -> Tìm kiếm sản phẩm
        public IActionResult DatSearch(string? datKeyword)
        {
            ViewBag.DatKeyword = datKeyword;
            var datResult = string.IsNullOrWhiteSpace(datKeyword)
                ? new List<DatProduct>()
                : datProducts.Where(p => p.DatName.Contains(datKeyword, StringComparison.OrdinalIgnoreCase)).ToList();
            return View(datResult);
        }

        // GET: /DatProducts/DatHots  -> Danh sách bán chạy
        public IActionResult DatHots()
        {
            return View(datProducts.Where(p => p.DatIsHot).ToList());
        }

        // GET: /DatProducts/DatAbout  -> Giới thiệu
        public IActionResult DatAbout()
        {
            return View();
        }
    }
}
