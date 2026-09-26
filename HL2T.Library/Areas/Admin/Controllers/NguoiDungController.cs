using HL2T.Library.Data;
using HL2T.Library.Models;
using HL2T.Library.Services;
using HL2T.Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = VaiTroNames.Admin)]
public class NguoiDungController : Controller
{
    private readonly ThuVienDbContext _db;
    private readonly IAuthService _auth;

    public NguoiDungController(ThuVienDbContext db, IAuthService auth)
    {
        _db = db;
        _auth = auth;
    }

    public async Task<IActionResult> Index(int? maVaiTro, string? tuKhoa)
    {
        var q = _db.NguoiDungs.Include(n => n.VaiTro).AsNoTracking().AsQueryable();
        if (maVaiTro.HasValue) q = q.Where(n => n.MaVaiTro == maVaiTro);
        if (!string.IsNullOrWhiteSpace(tuKhoa))
            q = q.Where(n => n.HoTen.Contains(tuKhoa) || n.TenDangNhap.Contains(tuKhoa) || n.Email.Contains(tuKhoa));
        ViewBag.VaiTros = await _db.VaiTros.ToListAsync();
        ViewBag.ThongKe = await _db.NguoiDungs.GroupBy(n => n.VaiTro!.TenVaiTro)
                                   .Select(g => new { g.Key, SoLuong = g.Count() })
                                   .ToDictionaryAsync(x => x.Key, x => x.SoLuong);
        ViewBag.MaVaiTro = maVaiTro;
        ViewBag.TuKhoa = tuKhoa;
        return View(await q.OrderBy(n => n.MaVaiTro).ThenBy(n => n.HoTen).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        await NapVaiTroAsync();
        var docGia = await _db.VaiTros.FirstAsync(v => v.TenVaiTro == VaiTroNames.DocGia);
        return View("Form", new NguoiDungFormVM { MaVaiTro = docGia.MaVaiTro });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var n = await _db.NguoiDungs.FindAsync(id);
        if (n == null) return NotFound();
        await NapVaiTroAsync();
        return View("Form", new NguoiDungFormVM
        {
            MaNguoiDung = n.MaNguoiDung, TenDangNhap = n.TenDangNhap, HoTen = n.HoTen, Email = n.Email,
            SoDienThoai = n.SoDienThoai, DiaChi = n.DiaChi, MaVaiTro = n.MaVaiTro, TrangThai = n.TrangThai
        });
    }

    [HttpPost]
    public async Task<IActionResult> Save(NguoiDungFormVM vm)
    {
        if (vm.MaNguoiDung == 0 && string.IsNullOrWhiteSpace(vm.MatKhau))
            ModelState.AddModelError(nameof(vm.MatKhau), "Vui lòng nhập mật khẩu cho tài khoản mới.");
        if (await _db.NguoiDungs.AnyAsync(n => n.TenDangNhap == vm.TenDangNhap && n.MaNguoiDung != vm.MaNguoiDung))
            ModelState.AddModelError(nameof(vm.TenDangNhap), "Tên đăng nhập đã tồn tại.");
        if (await _db.NguoiDungs.AnyAsync(n => n.Email == vm.Email && n.MaNguoiDung != vm.MaNguoiDung))
            ModelState.AddModelError(nameof(vm.Email), "Email đã tồn tại.");
        if (vm.MaNguoiDung == User.LayMaNguoiDung() && !vm.TrangThai)
            ModelState.AddModelError(nameof(vm.TrangThai), "Không thể tự khóa tài khoản của chính mình.");
        if (!ModelState.IsValid)
        {
            await NapVaiTroAsync();
            return View("Form", vm);
        }

        NguoiDung? n;
        if (vm.MaNguoiDung == 0)
        {
            n = new NguoiDung { NgayTao = DateTime.Now };
            _db.NguoiDungs.Add(n);
        }
        else
        {
            n = await _db.NguoiDungs.FindAsync(vm.MaNguoiDung);
            if (n == null) return NotFound();
        }
        n.TenDangNhap = vm.TenDangNhap.Trim();
        n.HoTen = vm.HoTen.Trim();
        n.Email = vm.Email.Trim();
        n.SoDienThoai = vm.SoDienThoai;
        n.DiaChi = vm.DiaChi;
        n.MaVaiTro = vm.MaVaiTro;
        n.TrangThai = vm.TrangThai;
        if (!string.IsNullOrWhiteSpace(vm.MatKhau)) n.MatKhauHash = _auth.BamMatKhau(vm.MatKhau);

        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Lưu tài khoản thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> KhoaMo(int id)
    {
        if (id == User.LayMaNguoiDung())
        {
            TempData["Loi"] = "Không thể tự khóa tài khoản của chính mình.";
            return RedirectToAction(nameof(Index));
        }
        var n = await _db.NguoiDungs.FindAsync(id);
        if (n == null) return NotFound();
        n.TrangThai = !n.TrangThai;
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = n.TrangThai ? $"Đã mở khóa tài khoản {n.TenDangNhap}." : $"Đã khóa tài khoản {n.TenDangNhap}.";
        return RedirectToAction(nameof(Index));
    }

    private async Task NapVaiTroAsync()
    {
        var ds = await _db.VaiTros.ToListAsync();
        ViewBag.VaiTros = new SelectList(ds.Select(v => new { v.MaVaiTro, Ten = VaiTroNames.HienThi(v.TenVaiTro) }), "MaVaiTro", "Ten");
    }
}
