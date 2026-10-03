using Microsoft.AspNetCore.Mvc;

namespace DAT_LESSON_14_LAYOUT.Areas.Admin.ViewComponents
{
    public class DatFooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
