using System.Security.Claims;
using HL2T.Library.Data;
using HL2T.Library.Models;
using HL2T.Library.Services;
using HL2T.Library.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _auth;
    private readonly ThuVienDbContext _db;

    public AccountController(IAuthService auth, ThuVienDbContext db)
    {
        _auth = auth;
        _db = db;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
        return View(new DangNhapVM { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Login(DangNhapVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var (nd, loi) = await _auth.XacThucAsync(vm.TenDangNhap.Trim(), vm.MatKhau);
        if (nd == null)
        {
            ModelState.AddModelError(string.Empty, loi!);
            return View(vm);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, nd.MaNguoiDung.ToString()),
            new(ClaimTypes.Name, nd.TenDangNhap),
            new(ClaimTypes.GivenName, nd.HoTen),
            new(ClaimTypes.Role, nd.VaiTro!.TenVaiTro)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = vm.GhiNho });

        if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
            return Redirect(vm.ReturnUrl);

        return nd.VaiTro.TenVaiTro == VaiTroNames.DocGia
            ? RedirectToAction("Index", "Home")
            : RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied() => View();

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var nd = await _db.NguoiDungs.FindAsync(User.LayMaNguoiDung());
        if (nd == null) return NotFound();
        return View(new ThongTinCaNhanVM
        {
            TenDangNhap = nd.TenDangNhap, HoTen = nd.HoTen, Email = nd.Email,
            SoDienThoai = nd.SoDienThoai, DiaChi = nd.DiaChi
        });
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Profile(ThongTinCaNhanVM vm)
    {
        var id = User.LayMaNguoiDung();
        if (await _db.NguoiDungs.AnyAsync(n => n.Email == vm.Email && n.MaNguoiDung != id))
            ModelState.AddModelError(nameof(vm.Email), "Email đã được sử dụng.");
        if (!ModelState.IsValid) return View(vm);

        var nd = await _db.NguoiDungs.FindAsync(id);
        if (nd == null) return NotFound();
        nd.HoTen = vm.HoTen; nd.Email = vm.Email; nd.SoDienThoai = vm.SoDienThoai; nd.DiaChi = vm.DiaChi;
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Cập nhật thông tin thành công.";
        return RedirectToAction(nameof(Profile));
    }

    [Authorize]
    public IActionResult DoiMatKhau() => View(new DoiMatKhauVM());

    [HttpPost, Authorize]
    public async Task<IActionResult> DoiMatKhau(DoiMatKhauVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var kq = await _auth.DoiMatKhauAsync(User.LayMaNguoiDung(), vm.MatKhauCu, vm.MatKhauMoi);
        if (!kq.ThanhCong)
        {
            ModelState.AddModelError(string.Empty, kq.ThongBao);
            return View(vm);
        }
        TempData["ThongBao"] = kq.ThongBao;
        return RedirectToAction(nameof(Profile));
    }
}
