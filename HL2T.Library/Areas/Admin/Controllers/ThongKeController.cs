using HL2T.Library.Models;
using HL2T.Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HL2T.Library.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = VaiTroNames.QuanTri)]
public class ThongKeController : Controller
{
    private readonly IThongKeService _tk;
    public ThongKeController(IThongKeService tk) => _tk = tk;

    public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay)
    {
        var (tu, den) = KhoangThoiGian(tuNgay, denNgay);
        if (tu > den)
        {
            TempData["Loi"] = "Ngày bắt đầu phải trước ngày kết thúc.";
            (tu, den) = KhoangThoiGian(null, null);
        }
        return View(await _tk.ThongKeAsync(tu, den));
    }

    public async Task<IActionResult> XuatExcel(DateTime? tuNgay, DateTime? denNgay)
    {
        var (tu, den) = KhoangThoiGian(tuNgay, denNgay);
        var vm = await _tk.ThongKeAsync(tu, den);
        return File(_tk.XuatExcel(vm), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"ThongKe_ThuVienHL2T_{tu:yyyyMMdd}_{den:yyyyMMdd}.xlsx");
    }

    private static (DateTime, DateTime) KhoangThoiGian(DateTime? tu, DateTime? den)
    {
        var d = den?.Date ?? DateTime.Today;
        var t = tu?.Date ?? new DateTime(d.Year, d.Month, 1).AddMonths(-5);
        return (t, d);
    }
}
