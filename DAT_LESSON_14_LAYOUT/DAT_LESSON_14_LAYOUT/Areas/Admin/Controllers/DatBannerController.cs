using DAT_LESSON_14_LAYOUT.Data;
using DAT_LESSON_14_LAYOUT.Helpers;
using DAT_LESSON_14_LAYOUT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAT_LESSON_14_LAYOUT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DatBannerController : Controller
    {
        private readonly DatAppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DatBannerController(DatAppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /Admin/DatBanner
        public async Task<IActionResult> Index(string? keyword)
        {
            var query = _db.DatBanners.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            ViewBag.Keyword = keyword;
            return View(await query.OrderByDescending(x => x.Id).ToListAsync());
        }

        // GET: /Admin/DatBanner/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.DatBanners.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /Admin/DatBanner/Create
        public IActionResult Create() => View(new DatBanner());

        // POST: /Admin/DatBanner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Status,Priority,Description")] DatBanner model, IFormFile? imageFile)
        {
            await DatValidate(model, imageFile);
            if (!ModelState.IsValid) return View(model);

            model.Name = model.Name.Trim();
            model.Image = await DatFileHelper.SaveAsync(imageFile, _env, "banner");

            _db.DatBanners.Add(model);
            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Thêm banner thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DatBanner/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.DatBanners.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Admin/DatBanner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Priority,Description")] DatBanner model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();
            var item = await _db.DatBanners.FindAsync(id);
            if (item == null) return NotFound();

            await DatValidate(model, imageFile);
            if (!ModelState.IsValid)
            {
                model.Image = item.Image;
                return View(model);
            }

            item.Name = model.Name.Trim();
            item.Status = model.Status;
            item.Priority = model.Priority;
            item.Description = model.Description;
            if (imageFile != null && imageFile.Length > 0)
            {
                DatFileHelper.Delete(item.Image, _env);
                item.Image = await DatFileHelper.SaveAsync(imageFile, _env, "banner");
            }

            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Cập nhật banner thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DatBanner/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.DatBanners.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Admin/DatBanner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.DatBanners.FindAsync(id);
            if (item == null) return NotFound();

            DatFileHelper.Delete(item.Image, _env);
            _db.DatBanners.Remove(item);
            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Xóa banner thành công";
            return RedirectToAction(nameof(Index));
        }

        // Kiểm tra: tên không trùng + file ảnh hợp lệ
        private async Task DatValidate(DatBanner model, IFormFile? imageFile)
        {
            var name = model.Name?.Trim() ?? "";
            if (name.Length > 0 && await _db.DatBanners.AnyAsync(x => x.Name == name && x.Id != model.Id))
                ModelState.AddModelError(nameof(DatBanner.Name), "Tên banner đã tồn tại");

            var imgError = DatFileHelper.Validate(imageFile);
            if (imgError != null)
                ModelState.AddModelError(nameof(DatBanner.Image), imgError);
        }
    }
}
