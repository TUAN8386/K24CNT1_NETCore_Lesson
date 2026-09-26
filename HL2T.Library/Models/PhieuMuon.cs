using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HL2T.Library.Models;

[Table("PhieuMuon")]
public class PhieuMuon
{
    [Key]
    public int MaPhieuMuon { get; set; }

    public int MaDocGia { get; set; }

    public int? MaThuThu { get; set; }

    public DateTime NgayLap { get; set; } = DateTime.Now;

    [Column(TypeName = "date")]
    public DateTime? NgayMuon { get; set; }

    [Column(TypeName = "date")]
    public DateTime? NgayHenTra { get; set; }

    public bool DaGiaHan { get; set; }

    [Required, StringLength(20)]
    public string TrangThai { get; set; } = TrangThaiPhieu.ChoDuyet;

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey(nameof(MaDocGia))]
    public NguoiDung? DocGia { get; set; }

    [ForeignKey(nameof(MaThuThu))]
    public NguoiDung? ThuThu { get; set; }

    public ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

    public ICollection<BienLaiPhat> BienLaiPhats { get; set; } = new List<BienLaiPhat>();

    [NotMapped]
    public string MaHienThi => $"PM{MaPhieuMuon:D6}";

    public bool KiemTraQuaHan() =>
        (TrangThai == TrangThaiPhieu.DangMuon || TrangThai == TrangThaiPhieu.QuaHan)
        && NgayHenTra.HasValue && NgayHenTra.Value.Date < DateTime.Today;

    public int SoNgayTre(DateTime ngayTra) =>
        NgayHenTra.HasValue ? Math.Max(0, (ngayTra.Date - NgayHenTra.Value.Date).Days) : 0;
}

public static class TrangThaiPhieu
{
    public const string ChoDuyet = "ChoDuyet";
    public const string DangMuon = "DangMuon";
    public const string QuaHan = "QuaHan";
    public const string DaTra = "DaTra";
    public const string DaHuy = "DaHuy";

    public static readonly string[] TatCa = { ChoDuyet, DangMuon, QuaHan, DaTra, DaHuy };

    public static string HienThi(string s) => s switch
    {
        ChoDuyet => "Chờ duyệt",
        DangMuon => "Đang mượn",
        QuaHan => "Quá hạn",
        DaTra => "Đã trả",
        DaHuy => "Đã hủy",
        _ => s
    };

    public static string Badge(string s) => s switch
    {
        ChoDuyet => "bg-warning text-dark",
        DangMuon => "bg-primary",
        QuaHan => "bg-danger",
        DaTra => "bg-success",
        _ => "bg-secondary"
    };
}
