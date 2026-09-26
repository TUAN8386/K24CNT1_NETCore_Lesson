using System.Diagnostics;
using DatNetCoreLAB6_EF.Data;
using DatNetCoreLAB6_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Controllers
{
    public class DatHomeController : Controller
    {
        private readonly DatAppDbContext _datContext;

        public DatHomeController(DatAppDbContext datContext)
        {
            _datContext = datContext;
        }

        // Bài 4: Trang chủ hiển thị banner (slider) + sản phẩm mới
        public async Task<IActionResult> DatIndex()
        {
            var datModel = new DatHomeViewModel
            {
                DatBanners = await _datContext.DatBanners
                    .Where(datB => datB.Status == 1)
                    .OrderByDescending(datB => datB.CreatedDate)
                    .ToListAsync(),
                DatProducts = await _datContext.DatProducts
                    .Include(datP => datP.Category)
                    .Where(datP => datP.Status == 1)
                    .OrderByDescending(datP => datP.CreatedDate)
                    .Take(8)
                    .ToListAsync()
            };
            return View(datModel);
        }

        // Bài 2: Danh sách sản phẩm dạng cột
        public async Task<IActionResult> DatProduct()
        {
            var datProducts = await _datContext.DatProducts
                .Include(datP => datP.Category)
                .Where(datP => datP.Status == 1)
                .OrderByDescending(datP => datP.CreatedDate)
                .ToListAsync();
            return View(datProducts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult DatError()
        {
            return View(new DatErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
