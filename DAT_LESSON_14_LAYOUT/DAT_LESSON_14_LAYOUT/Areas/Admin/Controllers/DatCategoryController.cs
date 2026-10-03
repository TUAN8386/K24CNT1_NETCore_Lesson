using DAT_LESSON_14_LAYOUT.Data;
using DAT_LESSON_14_LAYOUT.Helpers;
using DAT_LESSON_14_LAYOUT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAT_LESSON_14_LAYOUT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DatCategoryController : Controller
    {
        private readonly DatAppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DatCategoryController(DatAppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /Admin/DatCategory
        public async Task<IActionResult> Index(string? keyword)
        {
            var query = _db.DatCategories.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            ViewBag.Keyword = keyword;
            return View(await query.OrderByDescending(x => x.Id).ToListAsync());
        }

        // GET: /Admin/DatCategory/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.DatCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            ViewBag.DatProductCount = await _db.DatProducts.CountAsync(p => p.CategoryId == id);
            return View(item);
        }

        // GET: /Admin/DatCategory/Create
        public IActionResult Create() => View(new DatCategory());

        // POST: /Admin/DatCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Status,Description")] DatCategory model, IFormFile? imageFile)
        {
            await DatValidate(model, imageFile);
            if (!ModelState.IsValid) return View(model);

            model.Name = model.Name.Trim();
            model.CreatedDate = DateTime.Today;
            model.Image = await DatFileHelper.SaveAsync(imageFile, _env, "category");

            _db.DatCategories.Add(model);
            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Thêm danh mục thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DatCategory/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.DatCategories.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Admin/DatCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Description")] DatCategory model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();
            var item = await _db.DatCategories.FindAsync(id);
            if (item == null) return NotFound();

            await DatValidate(model, imageFile);
            if (!ModelState.IsValid)
            {
                model.Image = item.Image;
                model.CreatedDate = item.CreatedDate;
                return View(model);
            }

            item.Name = model.Name.Trim();
            item.Status = model.Status;
            item.Description = model.Description;
            if (imageFile != null && imageFile.Length > 0)
            {
                DatFileHelper.Delete(item.Image, _env);
                item.Image = await DatFileHelper.SaveAsync(imageFile, _env, "category");
            }

            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Cập nhật danh mục thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DatCategory/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.DatCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            ViewBag.DatProductCount = await _db.DatProducts.CountAsync(p => p.CategoryId == id);
            return View(item);
        }

        // POST: /Admin/DatCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.DatCategories.FindAsync(id);
            if (item == null) return NotFound();

            if (await _db.DatProducts.AnyAsync(p => p.CategoryId == id))
            {
                TempData["DatError"] = "Không thể xóa: danh mục đang có sản phẩm";
                return RedirectToAction(nameof(Index));
            }

            DatFileHelper.Delete(item.Image, _env);
            _db.DatCategories.Remove(item);
            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Xóa danh mục thành công";
            return RedirectToAction(nameof(Index));
        }

        // Kiểm tra: tên không trùng + file ảnh hợp lệ
        private async Task DatValidate(DatCategory model, IFormFile? imageFile)
        {
            var name = model.Name?.Trim() ?? "";
            if (name.Length > 0 && await _db.DatCategories.AnyAsync(x => x.Name == name && x.Id != model.Id))
                ModelState.AddModelError(nameof(DatCategory.Name), "Tên danh mục đã tồn tại");

            var imgError = DatFileHelper.Validate(imageFile);
            if (imgError != null)
                ModelState.AddModelError(nameof(DatCategory.Image), imgError);
        }
    }
}
