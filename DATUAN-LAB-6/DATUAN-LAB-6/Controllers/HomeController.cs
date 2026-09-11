using Microsoft.AspNetCore.Mvc;
using DATUAN_LAB_6.Models;

namespace DATUAN_LAB_6.Controllers
{
    public class HomeController : Controller
    {
        private Product _product = new Product { Name = string.Empty, Image = string.Empty };
    
        public IActionResult Index()
        {
            var newProducts = _product.GetProductList();
            return View(newProducts);
        }
    }
}