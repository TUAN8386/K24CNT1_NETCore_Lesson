using HL2T.Library.Data;
using HL2T.Library.Models;
using HL2T.Library.Services;
using HL2T.Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = VaiTroNames.QuanTri)]
public class SachController : Controller
{
    private readonly ISachService _sach;
    private readonly ThuVienDbContext _db;

    public SachController(ISachService sach, ThuVienDbContext db)
    {
        _sach = sach;
        _db = db;
    }

    public async Task<IActionResult> Index([FromQuery] TimKiemSachVM dk)
    {
        dk.KichThuocTrang = 15;
        return View(await _sach.TimKiemAsync(dk));
    }

    public async Task<IActionResult> Create()
    {
        await NapDanhMucAsync();
        return View("Form", new SachFormVM());
    }

    public async Task<IActionResult> Edit(int id)
    {
        var s = await _sach.LayTheoMaAsync(id);
        if (s == null) return NotFound();
        await NapDanhMucAsync();
        return View("Form", new SachFormVM
        {
            MaSach = s.MaSach, ISBN = s.ISBN, TenSach = s.TenSach, TacGia = s.TacGia, NhaXuatBan = s.NhaXuatBan,
            NamXuatBan = s.NamXuatBan, GiaBia = s.GiaBia, MoTa = s.MoTa, TongSoLuong = s.TongSoLuong,
            MaDanhMuc = s.MaDanhMuc, AnhBiaHienTai = s.AnhBia
        });
    }

    [HttpPost]
    public async Task<IActionResult> Save(SachFormVM vm)
    {
        if (!ModelState.IsValid)
        {
            await NapDanhMucAsync();
            return View("Form", vm);
        }
        var kq = vm.MaSach == 0 ? await _sach.ThemAsync(vm) : await _sach.CapNhatAsync(vm);
        if (!kq.ThanhCong)
        {
            ModelState.AddModelError(string.Empty, kq.ThongBao);
            await NapDanhMucAsync();
            return View("Form", vm);
        }
        TempData["ThongBao"] = kq.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var kq = await _sach.XoaAsync(id);
        TempData[kq.ThanhCong ? "ThongBao" : "Loi"] = kq.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    private async Task NapDanhMucAsync() =>
        ViewBag.DanhMucs = new SelectList(await _db.DanhMucSachs.OrderBy(d => d.TenDanhMuc).ToListAsync(), "MaDanhMuc", "TenDanhMuc");
}
