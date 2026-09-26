using HL2T.Library.Data;
using HL2T.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = VaiTroNames.QuanTri)]
public class DanhMucController : Controller
{
    private readonly ThuVienDbContext _db;
    public DanhMucController(ThuVienDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.SoSach = await _db.Sachs.GroupBy(s => s.MaDanhMuc)
                                  .Select(g => new { g.Key, SoLuong = g.Count() })
                                  .ToDictionaryAsync(x => x.Key, x => x.SoLuong);
        return View(await _db.DanhMucSachs.OrderBy(d => d.TenDanhMuc).ToListAsync());
    }

    public IActionResult Create() => View("Form", new DanhMucSach());

    public async Task<IActionResult> Edit(int id)
    {
        var dm = await _db.DanhMucSachs.FindAsync(id);
        return dm == null ? NotFound() : View("Form", dm);
    }

    [HttpPost]
    public async Task<IActionResult> Save(DanhMucSach dm)
    {
        ModelState.Remove(nameof(DanhMucSach.Sachs));
        if (await _db.DanhMucSachs.AnyAsync(d => d.TenDanhMuc == dm.TenDanhMuc && d.MaDanhMuc != dm.MaDanhMuc))
            ModelState.AddModelError(nameof(dm.TenDanhMuc), "Tên danh mục đã tồn tại.");
        if (!ModelState.IsValid) return View("Form", dm);

        if (dm.MaDanhMuc == 0) _db.DanhMucSachs.Add(dm);
        else _db.DanhMucSachs.Update(dm);
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Lưu danh mục thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var dm = await _db.DanhMucSachs.Include(d => d.Sachs).FirstOrDefaultAsync(d => d.MaDanhMuc == id);
        if (dm == null) return NotFound();
        if (dm.Sachs.Any())
            TempData["Loi"] = "Danh mục đang có sách, không thể xóa.";
        else
        {
            _db.DanhMucSachs.Remove(dm);
            await _db.SaveChangesAsync();
            TempData["ThongBao"] = "Đã xóa danh mục.";
        }
        return RedirectToAction(nameof(Index));
    }
}
