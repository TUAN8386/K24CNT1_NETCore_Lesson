using HL2T.Library.Services;
using HL2T.Library.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HL2T.Library.Controllers;

/// <summary>Tra cứu sách – phân hệ độc giả (không bắt buộc đăng nhập).</summary>
public class SachController : Controller
{
    private readonly ISachService _sach;
    public SachController(ISachService sach) => _sach = sach;

    public async Task<IActionResult> Index([FromQuery] TimKiemSachVM dk) =>
        View(await _sach.TimKiemAsync(dk));

    public async Task<IActionResult> ChiTiet(int id)
    {
        var sach = await _sach.LayTheoMaAsync(id);
        return sach == null ? NotFound() : View(sach);
    }
}
