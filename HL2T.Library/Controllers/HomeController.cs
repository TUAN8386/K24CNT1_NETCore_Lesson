using System.Diagnostics;
using HL2T.Library.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Controllers;

public record DanhMucThongKe(int MaDanhMuc, string TenDanhMuc, int SoSach);

public class HomeController : Controller
{
    private readonly ThuVienDbContext _db;
    public HomeController(ThuVienDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.DanhMucs = await _db.DanhMucSachs.OrderByDescending(d => d.Sachs.Count).Take(8)
                                    .Select(d => new DanhMucThongKe(d.MaDanhMuc, d.TenDanhMuc, d.Sachs.Count)).ToListAsync();
        ViewBag.SachMoi = await _db.Sachs.Include(s => s.DanhMucSach).OrderByDescending(s => s.MaSach).Take(6).ToListAsync();
        ViewBag.SachMuonNhieu = await _db.Sachs
            .OrderByDescending(s => s.ChiTietPhieuMuons.Sum(c => c.SoLuong)).Take(6).ToListAsync();
        return View();
    }

    public IActionResult Error() =>
        View(model: Activity.Current?.Id ?? HttpContext.TraceIdentifier);
}
