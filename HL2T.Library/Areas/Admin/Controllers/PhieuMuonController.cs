using HL2T.Library.Data;
using HL2T.Library.Models;
using HL2T.Library.Services;
using HL2T.Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = VaiTroNames.QuanTri)]
public class PhieuMuonController : Controller
{
    private readonly IMuonTraService _muonTra;
    private readonly ThuVienDbContext _db;

    public PhieuMuonController(IMuonTraService muonTra, ThuVienDbContext db)
    {
        _muonTra = muonTra;
        _db = db;
    }

    public async Task<IActionResult> Index(string? trangThai, string? tuKhoa, int? chon)
    {
        await _muonTra.CapNhatTrangThaiAsync();
        var q = _db.PhieuMuons.Include(p => p.DocGia).Include(p => p.ChiTietPhieuMuons).AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(trangThai)) q = q.Where(p => p.TrangThai == trangThai);
        if (!string.IsNullOrWhiteSpace(tuKhoa))
            q = q.Where(p => p.DocGia!.HoTen.Contains(tuKhoa) || p.DocGia.TenDangNhap.Contains(tuKhoa));
        ViewBag.TrangThai = trangThai;
        ViewBag.TuKhoa = tuKhoa;

        if (chon.HasValue)
        {
            var phieu = await _muonTra.LayPhieuAsync(chon.Value);
            ViewBag.PhieuChon = phieu;
            if (phieu?.TrangThai == TrangThaiPhieu.ChoDuyet)
                ViewBag.DieuKien = await _muonTra.KiemTraDieuKienMuonAsync(phieu.MaDocGia,
                    phieu.ChiTietPhieuMuons.Sum(c => c.SoLuong), phieu.MaPhieuMuon);
        }
        return View(await q.OrderBy(p => p.TrangThai == TrangThaiPhieu.ChoDuyet ? 0 : 1)
                           .ThenByDescending(p => p.NgayLap).Take(200).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Duyet(int id)
    {
        var kq = await _muonTra.DuyetPhieuAsync(id, User.LayMaNguoiDung());
        TempData[kq.ThanhCong ? "ThongBao" : "Loi"] = kq.ThongBao;
        return RedirectToAction(nameof(Index), new { chon = id });
    }

    [HttpPost]
    public async Task<IActionResult> TuChoi(int id, string? lyDo)
    {
        var kq = await _muonTra.TuChoiPhieuAsync(id, User.LayMaNguoiDung(), lyDo);
        TempData[kq.ThanhCong ? "ThongBao" : "Loi"] = kq.ThongBao;
        return RedirectToAction(nameof(Index), new { chon = id });
    }

    [HttpPost]
    public async Task<IActionResult> GiaHan(int id)
    {
        var kq = await _muonTra.GiaHanAsync(id);
        TempData[kq.ThanhCong ? "ThongBao" : "Loi"] = kq.ThongBao;
        return RedirectToAction(nameof(Index), new { chon = id });
    }

    public IActionResult LapPhieu() => View(new LapPhieuVM());

    [HttpPost]
    public async Task<IActionResult> LapPhieu(LapPhieuVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var docGia = await _db.NguoiDungs.FirstOrDefaultAsync(n => n.TenDangNhap == vm.DocGia.Trim() || n.Email == vm.DocGia.Trim());
        if (docGia == null)
        {
            ModelState.AddModelError(nameof(vm.DocGia), "Không tìm thấy độc giả.");
            return View(vm);
        }
        var maSachs = vm.DanhSachMaSach.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(x => int.TryParse(x.Trim().TrimStart('S', 's'), out var m) ? m : 0)
                         .Where(m => m > 0).ToList();
        var kq = await _muonTra.LapPhieuTaiQuayAsync(docGia.MaNguoiDung, maSachs, User.LayMaNguoiDung());
        if (!kq.ThanhCong)
        {
            ModelState.AddModelError(string.Empty, kq.ThongBao);
            return View(vm);
        }
        TempData["ThongBao"] = kq.ThongBao;
        return RedirectToAction(nameof(Index), new { chon = kq.Ma });
    }

    /// <summary>Màn hình trả sách: tìm phiếu và xem trước tiền phạt.</summary>
    public async Task<IActionResult> TraSach(int? id)
    {
        var vm = new TraSachVM();
        if (id.HasValue)
        {
            var phieu = await _muonTra.LayPhieuAsync(id.Value);
            if (phieu == null) TempData["Loi"] = "Không tìm thấy phiếu mượn.";
            else if (phieu.TrangThai != TrangThaiPhieu.DangMuon && phieu.TrangThai != TrangThaiPhieu.QuaHan)
                TempData["Loi"] = $"Phiếu {phieu.MaHienThi} không ở trạng thái đang mượn.";
            else
            {
                vm.MaPhieuMuon = phieu.MaPhieuMuon;
                vm.Phieu = phieu;
                vm.Dong = phieu.ChiTietPhieuMuons.Where(c => !c.DaTra).Select(c => new TraSachDongVM
                {
                    MaChiTiet = c.MaChiTiet, TenSach = c.Sach!.TenSach, GiaBia = c.Sach.GiaBia, SoLuong = c.SoLuong
                }).ToList();
                ViewBag.TienPhat = _muonTra.TinhTienPhat(phieu, vm.Dong, DateTime.Today);
            }
        }
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> TraSach(TraSachVM vm)
    {
        var kq = await _muonTra.TraSachAsync(vm.MaPhieuMuon, vm.Dong, User.LayMaNguoiDung());
        TempData[kq.ThanhCong ? "ThongBao" : "Loi"] = kq.ThongBao;
        if (kq.ThanhCong && kq.Ma.HasValue)
            return RedirectToAction("In", "BienLai", new { id = kq.Ma });
        return kq.ThanhCong ? RedirectToAction(nameof(Index), new { chon = vm.MaPhieuMuon })
                            : RedirectToAction(nameof(TraSach), new { id = vm.MaPhieuMuon });
    }
}
