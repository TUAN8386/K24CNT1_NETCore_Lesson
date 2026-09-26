using HL2T.Library.Models;

namespace HL2T.Library.ViewModels;

public class DashboardVM
{
    public int TongDauSach { get; set; }
    public int TongBanSach { get; set; }
    public int PhieuDangMuon { get; set; }
    public int PhieuChoDuyet { get; set; }
    public int PhieuQuaHan { get; set; }
    public int SoDocGia { get; set; }
    public List<(string Thang, int SoLuot)> LuotMuonTheoThang { get; set; } = new();
    public List<(string TenSach, int SoLuot)> TopSach { get; set; } = new();
    public List<PhieuMuon> PhieuGanDay { get; set; } = new();
}

public class ThongKeThangVM
{
    public string Thang { get; set; } = string.Empty;
    public int LuotMuon { get; set; }
    public int LuotTra { get; set; }
    public int PhieuQuaHan { get; set; }
    public decimal TienPhat { get; set; }
}

public class ThongKeVM
{
    public DateTime TuNgay { get; set; }
    public DateTime DenNgay { get; set; }
    public List<ThongKeThangVM> TheoThang { get; set; } = new();
    public List<(string DanhMuc, int SoLuot)> TheoDanhMuc { get; set; } = new();
    public List<(string TenSach, int SoLuot)> TopSach { get; set; } = new();
}
