using ClosedXML.Excel;
using HL2T.Library.Data;
using HL2T.Library.Models;
using HL2T.Library.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HL2T.Library.Services;

public interface IThongKeService
{
    Task<DashboardVM> DashboardAsync();
    Task<ThongKeVM> ThongKeAsync(DateTime tuNgay, DateTime denNgay);
    byte[] XuatExcel(ThongKeVM vm);
}

public class ThongKeService : IThongKeService
{
    private readonly ThuVienDbContext _db;
    public ThongKeService(ThuVienDbContext db) => _db = db;

    public async Task<DashboardVM> DashboardAsync()
    {
        var vm = new DashboardVM
        {
            TongDauSach = await _db.Sachs.CountAsync(),
            TongBanSach = await _db.Sachs.SumAsync(s => (int?)s.TongSoLuong) ?? 0,
            PhieuDangMuon = await _db.PhieuMuons.CountAsync(p => p.TrangThai == TrangThaiPhieu.DangMuon || p.TrangThai == TrangThaiPhieu.QuaHan),
            PhieuChoDuyet = await _db.PhieuMuons.CountAsync(p => p.TrangThai == TrangThaiPhieu.ChoDuyet),
            PhieuQuaHan = await _db.PhieuMuons.CountAsync(p => p.TrangThai == TrangThaiPhieu.QuaHan),
            SoDocGia = await _db.NguoiDungs.CountAsync(n => n.VaiTro!.TenVaiTro == VaiTroNames.DocGia && n.TrangThai),
            PhieuGanDay = await _db.PhieuMuons.Include(p => p.DocGia).Include(p => p.ChiTietPhieuMuons)
                                  .OrderByDescending(p => p.NgayLap).Take(6).AsNoTracking().ToListAsync()
        };

        var dauKy = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-5);
        var ngayMuon = await _db.PhieuMuons.Where(p => p.NgayMuon >= dauKy)
                                           .Select(p => p.NgayMuon!.Value).ToListAsync();
        for (int i = 0; i < 6; i++)
        {
            var t = dauKy.AddMonths(i);
            vm.LuotMuonTheoThang.Add(($"{t:MM/yyyy}", ngayMuon.Count(d => d.Year == t.Year && d.Month == t.Month)));
        }

        vm.TopSach = await TopSachAsync(null, null, 5);
        return vm;
    }

    public async Task<ThongKeVM> ThongKeAsync(DateTime tuNgay, DateTime denNgay)
    {
        var vm = new ThongKeVM { TuNgay = tuNgay.Date, DenNgay = denNgay.Date };
        var den = denNgay.Date.AddDays(1);

        var muon = await _db.PhieuMuons.Where(p => p.NgayMuon >= tuNgay && p.NgayMuon < den)
            .Select(p => new { Ngay = p.NgayMuon!.Value, SoSach = p.ChiTietPhieuMuons.Sum(c => c.SoLuong), p.TrangThai, p.NgayHenTra })
            .ToListAsync();
        var tra = await _db.ChiTietPhieuMuons.Where(c => c.NgayTraThucTe >= tuNgay && c.NgayTraThucTe < den)
            .Select(c => new { Ngay = c.NgayTraThucTe!.Value, c.SoLuong }).ToListAsync();
        var phat = await _db.BienLaiPhats.Where(b => b.NgayLap >= tuNgay && b.NgayLap < den && b.DaThanhToan)
            .Select(b => new { b.NgayLap, b.SoTien }).ToListAsync();

        for (var t = new DateTime(tuNgay.Year, tuNgay.Month, 1); t < den; t = t.AddMonths(1))
        {
            bool CungThang(DateTime d) => d.Year == t.Year && d.Month == t.Month;
            vm.TheoThang.Add(new ThongKeThangVM
            {
                Thang = $"{t:MM/yyyy}",
                LuotMuon = muon.Where(m => CungThang(m.Ngay)).Sum(m => m.SoSach),
                LuotTra = tra.Where(x => CungThang(x.Ngay)).Sum(x => x.SoLuong),
                PhieuQuaHan = muon.Count(m => CungThang(m.Ngay) && m.TrangThai == TrangThaiPhieu.QuaHan),
                TienPhat = phat.Where(p => CungThang(p.NgayLap)).Sum(p => p.SoTien)
            });
        }

        var theoDm = await _db.ChiTietPhieuMuons
            .Where(c => c.PhieuMuon!.NgayMuon >= tuNgay && c.PhieuMuon.NgayMuon < den)
            .GroupBy(c => c.Sach!.DanhMucSach!.TenDanhMuc)
            .Select(g => new { g.Key, SoLuot = g.Sum(x => x.SoLuong) })
            .OrderByDescending(x => x.SoLuot).ToListAsync();
        vm.TheoDanhMuc = theoDm.Select(x => (x.Key, x.SoLuot)).ToList();
        vm.TopSach = await TopSachAsync(tuNgay, den, 10);
        return vm;
    }

    private async Task<List<(string, int)>> TopSachAsync(DateTime? tu, DateTime? den, int top)
    {
        var q = _db.ChiTietPhieuMuons.Where(c => c.PhieuMuon!.NgayMuon != null);
        if (tu.HasValue) q = q.Where(c => c.PhieuMuon!.NgayMuon >= tu);
        if (den.HasValue) q = q.Where(c => c.PhieuMuon!.NgayMuon < den);
        var ds = await q
            .GroupBy(c => c.Sach!.TenSach)
            .Select(g => new { g.Key, SoLuot = g.Sum(x => x.SoLuong) })
            .OrderByDescending(x => x.SoLuot).Take(top).ToListAsync();
        return ds.Select(x => (x.Key, x.SoLuot)).ToList();
    }

    public byte[] XuatExcel(ThongKeVM vm)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Theo tháng");
        ws.Cell(1, 1).Value = $"BÁO CÁO HOẠT ĐỘNG THƯ VIỆN HL2T ({vm.TuNgay:dd/MM/yyyy} - {vm.DenNgay:dd/MM/yyyy})";
        ws.Range(1, 1, 1, 5).Merge().Style.Font.SetBold().Font.SetFontSize(14);
        string[] header = { "Tháng", "Lượt mượn", "Lượt trả", "Phiếu quá hạn", "Tiền phạt thu (đ)" };
        for (int i = 0; i < header.Length; i++) ws.Cell(3, i + 1).Value = header[i];
        ws.Range(3, 1, 3, 5).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightSteelBlue);
        int r = 4;
        foreach (var t in vm.TheoThang)
        {
            ws.Cell(r, 1).Value = t.Thang;
            ws.Cell(r, 2).Value = t.LuotMuon;
            ws.Cell(r, 3).Value = t.LuotTra;
            ws.Cell(r, 4).Value = t.PhieuQuaHan;
            ws.Cell(r, 5).Value = (double)t.TienPhat;
            ws.Cell(r, 5).Style.NumberFormat.Format = "#,##0";
            r++;
        }
        ws.Cell(r, 1).Value = "Tổng";
        ws.Cell(r, 2).FormulaA1 = $"SUM(B4:B{r - 1})";
        ws.Cell(r, 3).FormulaA1 = $"SUM(C4:C{r - 1})";
        ws.Cell(r, 4).FormulaA1 = $"SUM(D4:D{r - 1})";
        ws.Cell(r, 5).FormulaA1 = $"SUM(E4:E{r - 1})";
        ws.Cell(r, 5).Style.NumberFormat.Format = "#,##0";
        ws.Range(r, 1, r, 5).Style.Font.SetBold();
        ws.Columns().AdjustToContents();

        var ws2 = wb.Worksheets.Add("Top sách");
        ws2.Cell(1, 1).Value = "STT"; ws2.Cell(1, 2).Value = "Tên sách"; ws2.Cell(1, 3).Value = "Lượt mượn";
        ws2.Range(1, 1, 1, 3).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightSteelBlue);
        for (int i = 0; i < vm.TopSach.Count; i++)
        {
            ws2.Cell(i + 2, 1).Value = i + 1;
            ws2.Cell(i + 2, 2).Value = vm.TopSach[i].TenSach;
            ws2.Cell(i + 2, 3).Value = vm.TopSach[i].SoLuot;
        }
        ws2.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
