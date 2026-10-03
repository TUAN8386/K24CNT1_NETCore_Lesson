using DAT_LESSON_14_LAYOUT.Data;
using DAT_LESSON_14_LAYOUT.Helpers;
using DAT_LESSON_14_LAYOUT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DAT_LESSON_14_LAYOUT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DatProductController : Controller
    {
        private readonly DatAppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DatProductController(DatAppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /Admin/DatProduct
        public async Task<IActionResult> Index(string? keyword)
        {
            var query = _db.DatProducts.Include(p => p.Category).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            ViewBag.Keyword = keyword;
            return View(await query.OrderByDescending(x => x.Id).ToListAsync());
        }

        // GET: /Admin/DatProduct/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.DatProducts.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /Admin/DatProduct/Create
        public async Task<IActionResult> Create()
        {
            await DatLoadCategories(null);
            return View(new DatProduct());
        }

        // POST: /Admin/DatProduct/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Price,SalePrice,Status,CategoryId,Description")] DatProduct model, IFormFile? imageFile)
        {
            await DatValidate(model, imageFile);
            if (!ModelState.IsValid)
            {
                await DatLoadCategories(model.CategoryId);
                return View(model);
            }

            model.Name = model.Name.Trim();
            model.CreatedDate = DateTime.Today;
            model.Image = await DatFileHelper.SaveAsync(imageFile, _env, "product");

            _db.DatProducts.Add(model);
            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Thêm sản phẩm thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DatProduct/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.DatProducts.FindAsync(id);
            if (item == null) return NotFound();
            await DatLoadCategories(item.CategoryId);
            return View(item);
        }

        // POST: /Admin/DatProduct/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,Price,SalePrice,Status,CategoryId,Description")] DatProduct model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();
            var item = await _db.DatProducts.FindAsync(id);
            if (item == null) return NotFound();

            await DatValidate(model, imageFile);
            if (!ModelState.IsValid)
            {
                model.Image = item.Image;
                model.CreatedDate = item.CreatedDate;
                await DatLoadCategories(model.CategoryId);
                return View(model);
            }

            item.Name = model.Name.Trim();
            item.Price = model.Price;
            item.SalePrice = model.SalePrice;
            item.Status = model.Status;
            item.CategoryId = model.CategoryId;
            item.Description = model.Description;
            if (imageFile != null && imageFile.Length > 0)
            {
                DatFileHelper.Delete(item.Image, _env);
                item.Image = await DatFileHelper.SaveAsync(imageFile, _env, "product");
            }

            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Cập nhật sản phẩm thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DatProduct/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.DatProducts.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Admin/DatProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.DatProducts.FindAsync(id);
            if (item == null) return NotFound();

            DatFileHelper.Delete(item.Image, _env);
            _db.DatProducts.Remove(item);
            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Xóa sản phẩm thành công";
            return RedirectToAction(nameof(Index));
        }

        private async Task DatLoadCategories(int? selectedId)
        {
            var categories = await _db.DatCategories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
            ViewBag.DatCategoryList = new SelectList(categories, nameof(DatCategory.Id), nameof(DatCategory.Name), selectedId);
        }

        // Kiểm tra: tên không trùng, danh mục tồn tại, giá KM <= giá, ảnh hợp lệ
        private async Task DatValidate(DatProduct model, IFormFile? imageFile)
        {
            var name = model.Name?.Trim() ?? "";
            if (name.Length > 0 && await _db.DatProducts.AnyAsync(x => x.Name == name && x.Id != model.Id))
                ModelState.AddModelError(nameof(DatProduct.Name), "Tên sản phẩm đã tồn tại");

            if (model.CategoryId.HasValue && !await _db.DatCategories.AnyAsync(c => c.Id == model.CategoryId))
                ModelState.AddModelError(nameof(DatProduct.CategoryId), "Danh mục không tồn tại");

            if (model.Price.HasValue && model.SalePrice > model.Price)
                ModelState.AddModelError(nameof(DatProduct.SalePrice), "Giá khuyến mãi không được lớn hơn giá bán");

            var imgError = DatFileHelper.Validate(imageFile);
            if (imgError != null)
                ModelState.AddModelError(nameof(DatProduct.Image), imgError);
        }
    }
}
