using Microsoft.AspNetCore.Mvc;

namespace DAT_LESSON_14_LAYOUT.Areas.Admin.ViewComponents
{
    public class DatHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
