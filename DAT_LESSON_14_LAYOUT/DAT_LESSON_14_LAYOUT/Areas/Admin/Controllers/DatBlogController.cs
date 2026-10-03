using DAT_LESSON_14_LAYOUT.Data;
using DAT_LESSON_14_LAYOUT.Helpers;
using DAT_LESSON_14_LAYOUT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAT_LESSON_14_LAYOUT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DatBlogController : Controller
    {
        private readonly DatAppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DatBlogController(DatAppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /Admin/DatBlog
        public async Task<IActionResult> Index(string? keyword)
        {
            var query = _db.DatBlogs.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            ViewBag.Keyword = keyword;
            return View(await query.OrderByDescending(x => x.Id).ToListAsync());
        }

        // GET: /Admin/DatBlog/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.DatBlogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /Admin/DatBlog/Create
        public IActionResult Create() => View(new DatBlog());

        // POST: /Admin/DatBlog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Status,Description")] DatBlog model, IFormFile? imageFile)
        {
            await DatValidate(model, imageFile);
            if (!ModelState.IsValid) return View(model);

            model.Name = model.Name.Trim();
            model.CreatedDate = DateTime.Today;
            model.Image = await DatFileHelper.SaveAsync(imageFile, _env, "blog");

            _db.DatBlogs.Add(model);
            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Thêm bài viết thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DatBlog/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.DatBlogs.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Admin/DatBlog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Description")] DatBlog model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();
            var item = await _db.DatBlogs.FindAsync(id);
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
                item.Image = await DatFileHelper.SaveAsync(imageFile, _env, "blog");
            }

            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Cập nhật bài viết thành công";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DatBlog/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.DatBlogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /Admin/DatBlog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.DatBlogs.FindAsync(id);
            if (item == null) return NotFound();

            DatFileHelper.Delete(item.Image, _env);
            _db.DatBlogs.Remove(item);
            await _db.SaveChangesAsync();
            TempData["DatSuccess"] = "Xóa bài viết thành công";
            return RedirectToAction(nameof(Index));
        }

        // Kiểm tra: tên không trùng + file ảnh hợp lệ
        private async Task DatValidate(DatBlog model, IFormFile? imageFile)
        {
            var name = model.Name?.Trim() ?? "";
            if (name.Length > 0 && await _db.DatBlogs.AnyAsync(x => x.Name == name && x.Id != model.Id))
                ModelState.AddModelError(nameof(DatBlog.Name), "Tên bài viết đã tồn tại");

            var imgError = DatFileHelper.Validate(imageFile);
            if (imgError != null)
                ModelState.AddModelError(nameof(DatBlog.Image), imgError);
        }
    }
}
