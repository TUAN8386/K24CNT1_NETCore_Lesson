using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HL2T.Library.Models;

[Table("BienLaiPhat")]
public class BienLaiPhat
{
    [Key]
    public int MaBienLai { get; set; }

    public int MaPhieuMuon { get; set; }

    public int MaDocGia { get; set; }

    [Required, StringLength(255)]
    public string LyDo { get; set; } = string.Empty;

    [Column(TypeName = "decimal(12,0)")]
    public decimal SoTien { get; set; }

    public DateTime NgayLap { get; set; } = DateTime.Now;

    public bool DaThanhToan { get; set; }

    [ForeignKey(nameof(MaPhieuMuon))]
    public PhieuMuon? PhieuMuon { get; set; }

    [ForeignKey(nameof(MaDocGia))]
    public NguoiDung? DocGia { get; set; }

    [NotMapped]
    public string MaHienThi => $"BL{MaBienLai:D6}";

    public void XacNhanThanhToan() => DaThanhToan = true;
}
