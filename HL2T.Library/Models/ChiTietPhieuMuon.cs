using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HL2T.Library.Models;

[Table("ChiTietPhieuMuon")]
public class ChiTietPhieuMuon
{
    [Key]
    public int MaChiTiet { get; set; }

    public int MaPhieuMuon { get; set; }

    public int MaSach { get; set; }

    [Range(1, 5)]
    public int SoLuong { get; set; } = 1;

    [Column(TypeName = "date")]
    public DateTime? NgayTraThucTe { get; set; }

    [StringLength(50)]
    public string? TinhTrangKhiTra { get; set; }

    [ForeignKey(nameof(MaPhieuMuon))]
    public PhieuMuon? PhieuMuon { get; set; }

    [ForeignKey(nameof(MaSach))]
    public Sach? Sach { get; set; }

    [NotMapped]
    public bool DaTra => NgayTraThucTe.HasValue;
}

/// <summary>Tình trạng sách khi trả và tỷ lệ bồi thường theo giá bìa (QĐ5).</summary>
public static class TinhTrangSach
{
    public const string BinhThuong = "Bình thường";
    public const string HuHongNhe = "Hư hỏng nhẹ";
    public const string HuHongNang = "Hư hỏng nặng";
    public const string Mat = "Mất sách";

    public static readonly string[] TatCa = { BinhThuong, HuHongNhe, HuHongNang, Mat };

    public static decimal TyLeBoiThuong(string? tinhTrang) => tinhTrang switch
    {
        HuHongNhe => 0.1m,
        HuHongNang => 0.5m,
        Mat => 1.0m,
        _ => 0m
    };
}
