namespace HL2T.Library.Services;

/// <summary>Các tham số quy định nghiệp vụ (QĐ1–QĐ7), đọc từ appsettings.json.</summary>
public class QuyDinhThuVien
{
    public int SoSachMuonToiDa { get; set; } = 5;      // QĐ1
    public int SoNgayMuon { get; set; } = 14;          // QĐ1
    public int SoNgayGiaHan { get; set; } = 7;         // QĐ3
    public decimal TienPhatMoiNgay { get; set; } = 5000; // QĐ4
    public int SoNgayHuyPhieuChoDuyet { get; set; } = 3; // QĐ7
}

/// <summary>Kết quả trả về của các nghiệp vụ.</summary>
public record KetQua(bool ThanhCong, string ThongBao, int? Ma = null)
{
    public static KetQua Ok(string msg, int? ma = null) => new(true, msg, ma);
    public static KetQua Loi(string msg) => new(false, msg);
}
