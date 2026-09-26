using HL2T.Library.Models;
using HL2T.Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HL2T.Library.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = VaiTroNames.QuanTri)]
public class DashboardController : Controller
{
    private readonly IThongKeService _tk;
    private readonly IMuonTraService _muonTra;

    public DashboardController(IThongKeService tk, IMuonTraService muonTra)
    {
        _tk = tk;
        _muonTra = muonTra;
    }

    public async Task<IActionResult> Index()
    {
        await _muonTra.CapNhatTrangThaiAsync();
        return View(await _tk.DashboardAsync());
    }
}
