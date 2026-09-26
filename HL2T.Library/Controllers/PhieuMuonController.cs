using HL2T.Library.Data;
using HL2T.Library.Models;
using HL2T.Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Controllers;

/// <summary>Đăng ký mượn, gia hạn và lịch sử mượn trả của độc giả.</summary>
[Authorize(Roles = VaiTroNames.DocGia)]
public class PhieuMuonController : Controller
{
    private readonly IMuonTraService _muonTra;
    private readonly ThuVienDbContext _db;

    public PhieuMuonController(IMuonTraService muonTra, ThuVienDbContext db)
    {
        _muonTra = muonTra;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> DangKy(int maSach)
    {
        var kq = await _muonTra.DangKyMuonAsync(User.LayMaNguoiDung(), new List<int> { maSach });
        TempData[kq.ThanhCong ? "ThongBao" : "Loi"] = kq.ThongBao;
        return kq.ThanhCong ? RedirectToAction(nameof(LichSu)) : RedirectToAction("ChiTiet", "Sach", new { id = maSach });
    }

    public async Task<IActionResult> LichSu(string? trangThai)
    {
        await _muonTra.CapNhatTrangThaiAsync();
        var id = User.LayMaNguoiDung();
        var q = _db.PhieuMuons.Include(p => p.ChiTietPhieuMuons).ThenInclude(c => c.Sach)
                              .Include(p => p.BienLaiPhats)
                              .Where(p => p.MaDocGia == id);
        if (!string.IsNullOrEmpty(trangThai)) q = q.Where(p => p.TrangThai == trangThai);
        ViewBag.TrangThai = trangThai;
        ViewBag.NoPhat = await _db.BienLaiPhats.Where(b => b.MaDocGia == id && !b.DaThanhToan).SumAsync(b => (decimal?)b.SoTien) ?? 0;
        return View(await q.OrderByDescending(p => p.NgayLap).AsNoTracking().ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> GiaHan(int id)
    {
        var kq = await _muonTra.GiaHanAsync(id, User.LayMaNguoiDung());
        TempData[kq.ThanhCong ? "ThongBao" : "Loi"] = kq.ThongBao;
        return RedirectToAction(nameof(LichSu));
    }
}
