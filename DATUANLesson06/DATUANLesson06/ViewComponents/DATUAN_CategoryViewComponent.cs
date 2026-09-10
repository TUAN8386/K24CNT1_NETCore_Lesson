
using Microsoft.AspNetCore.Mvc;
using DATUANLesson06.Models;
using System.Collections.Generic;
using System.Linq;

namespace DATUANLesson06.ViewComponents
{
    public class DATUAN_CategoryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int? n)
        {
            int limit = n ?? 0;

            var categories = new List<DATUAN_Category>
            {
                new DATUAN_Category { CategoryId = 1, CategoryName = "Đồ gia dụng", IsActive = true },
                new DATUAN_Category { CategoryId = 2, CategoryName = "Thiết bị điện tử", IsActive = true },
                new DATUAN_Category { CategoryId = 3, CategoryName = "Quần áo thời trang", IsActive = true },
                new DATUAN_Category { CategoryId = 4, CategoryName = "Y tế & Sức khỏe", IsActive = true }
            };

            var result = categories.Where(c => c.CategoryId > limit).ToList();

            return View(result);
        }
    }
}