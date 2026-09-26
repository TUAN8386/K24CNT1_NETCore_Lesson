using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HL2T.Library.Models;

[Table("NguoiDung")]
public class NguoiDung
{
    [Key]
    public int MaNguoiDung { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string MatKhauHash { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required, StringLength(100), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(15)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [StringLength(255)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    public DateTime NgayTao { get; set; } = DateTime.Now;

    /// <summary>true: hoạt động, false: bị khóa</summary>
    public bool TrangThai { get; set; } = true;

    public int MaVaiTro { get; set; }

    [ForeignKey(nameof(MaVaiTro))]
    public VaiTro? VaiTro { get; set; }

    [InverseProperty(nameof(PhieuMuon.DocGia))]
    public ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();

    [InverseProperty(nameof(BienLaiPhat.DocGia))]
    public ICollection<BienLaiPhat> BienLaiPhats { get; set; } = new List<BienLaiPhat>();
}
