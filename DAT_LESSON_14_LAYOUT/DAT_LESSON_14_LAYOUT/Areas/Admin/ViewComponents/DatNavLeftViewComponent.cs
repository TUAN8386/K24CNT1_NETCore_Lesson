using Microsoft.AspNetCore.Mvc;

namespace DAT_LESSON_14_LAYOUT.Areas.Admin.ViewComponents
{
    // Một mục menu bên trái
    public record DatMenuItem(string Title, string Icon, string Controller, string Action = "Index");

    // Component menu trái: bỏ các menu mẫu của AdminLTE, thay bằng menu của dự án
    public class DatNavLeftViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menu = new List<DatMenuItem>
            {
                new("Dashboard", "fas fa-tachometer-alt", "DatDashboard"),
                new("Danh mục",  "fas fa-list",           "DatCategory"),
                new("Sản phẩm",  "fas fa-box-open",       "DatProduct"),
                new("Banner",    "fas fa-images",         "DatBanner"),
                new("Blog",      "fas fa-newspaper",      "DatBlog"),
            };

            ViewBag.DatCurrent = RouteData.Values["controller"]?.ToString();
            return View(menu);
        }
    }
}
