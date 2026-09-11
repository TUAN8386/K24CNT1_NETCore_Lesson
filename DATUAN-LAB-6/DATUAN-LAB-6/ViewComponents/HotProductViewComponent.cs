using Microsoft.AspNetCore.Mvc;
using DATUAN_LAB_6.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DATUAN_LAB_6.ViewComponents
{
    public class HotProductViewComponent : ViewComponent
    {
        private readonly Product _product;

        public HotProductViewComponent()
        {
            _product = new Product { Name = string.Empty, Image = string.Empty };
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hotProducts = _product.GetProductList().Where(p => p.IsHot).ToList();
            return View(hotProducts);
        }
    }
}