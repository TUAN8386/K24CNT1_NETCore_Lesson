using DatNetCoreLAB6_EF.Data;
using DatNetCoreLAB6_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Controllers
{
    // Bài 3: CRUD bảng DatCategory
    public class DatCategoriesController : Controller
    {
        private readonly DatAppDbContext _datContext;

        public DatCategoriesController(DatAppDbContext datContext)
        {
            _datContext = datContext;
        }

        // GET: DatCategories/DatIndex
        public async Task<IActionResult> DatIndex()
        {
            var datCategories = await _datContext.DatCategories
                .OrderByDescending(datC => datC.Id)
                .ToListAsync();
            return View(datCategories);
        }

        // GET: DatCategories/DatDetails/5
        public async Task<IActionResult> DatDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datCategory = await _datContext.DatCategories
                .Include(datC => datC.Products)
                .FirstOrDefaultAsync(datC => datC.Id == id);
            if (datCategory == null)
            {
                return NotFound();
            }

            return View(datCategory);
        }

        // GET: DatCategories/DatCreate
        public IActionResult DatCreate()
        {
            return View(new DatCategory());
        }

        // POST: DatCategories/DatCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatCreate([Bind("Name,Status")] DatCategory datCategory)
        {
            if (ModelState.IsValid)
            {
                datCategory.CreatedDate = DateTime.Now;
                _datContext.Add(datCategory);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Thêm mới danh mục thành công";
                return RedirectToAction(nameof(DatIndex));
            }
            return View(datCategory);
        }

        // GET: DatCategories/DatEdit/5
        public async Task<IActionResult> DatEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datCategory = await _datContext.DatCategories.FindAsync(id);
            if (datCategory == null)
            {
                return NotFound();
            }
            return View(datCategory);
        }

        // POST: DatCategories/DatEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatEdit(int id, [Bind("Id,Name,Status")] DatCategory datCategory)
        {
            if (id != datCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var datExisting = await _datContext.DatCategories.FindAsync(id);
                if (datExisting == null)
                {
                    return NotFound();
                }

                datExisting.Name = datCategory.Name;
                datExisting.Status = datCategory.Status;
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Cập nhật danh mục thành công";
                return RedirectToAction(nameof(DatIndex));
            }
            return View(datCategory);
        }

        // GET: DatCategories/DatDelete/5
        public async Task<IActionResult> DatDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datCategory = await _datContext.DatCategories
                .FirstOrDefaultAsync(datC => datC.Id == id);
            if (datCategory == null)
            {
                return NotFound();
            }

            ViewBag.DatProductCount = await _datContext.DatProducts.CountAsync(datP => datP.CategoryId == id);
            return View(datCategory);
        }

        // POST: DatCategories/DatDelete/5
        [HttpPost, ActionName("DatDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatDeleteConfirmed(int id)
        {
            var datCategory = await _datContext.DatCategories.FindAsync(id);
            if (datCategory != null)
            {
                if (await _datContext.DatProducts.AnyAsync(datP => datP.CategoryId == id))
                {
                    TempData["DatError"] = "Không thể xóa: danh mục đang có sản phẩm";
                    return RedirectToAction(nameof(DatIndex));
                }

                _datContext.DatCategories.Remove(datCategory);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Xóa danh mục thành công";
            }
            return RedirectToAction(nameof(DatIndex));
        }
    }
}
