using HL2T.Library.Data;
using HL2T.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = VaiTroNames.QuanTri)]
public class BienLaiController : Controller
{
    private readonly ThuVienDbContext _db;
    public BienLaiController(ThuVienDbContext db) => _db = db;

    public async Task<IActionResult> Index(bool? daThanhToan)
    {
        var q = _db.BienLaiPhats.Include(b => b.DocGia).AsNoTracking().AsQueryable();
        if (daThanhToan.HasValue) q = q.Where(b => b.DaThanhToan == daThanhToan);
        ViewBag.DaThanhToan = daThanhToan;
        return View(await q.OrderByDescending(b => b.NgayLap).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> ThanhToan(int id, string? returnTo)
    {
        var bl = await _db.BienLaiPhats.FindAsync(id);
        if (bl == null) return NotFound();
        bl.XacNhanThanhToan();
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã xác nhận thanh toán biên lai {bl.MaHienThi}.";
        return returnTo == "in" ? RedirectToAction(nameof(In), new { id }) : RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> In(int id)
    {
        var bl = await _db.BienLaiPhats.Include(b => b.DocGia)
                                       .Include(b => b.PhieuMuon).ThenInclude(p => p!.ChiTietPhieuMuons).ThenInclude(c => c.Sach)
                                       .FirstOrDefaultAsync(b => b.MaBienLai == id);
        return bl == null ? NotFound() : View(bl);
    }
}
