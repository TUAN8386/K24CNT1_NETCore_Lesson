using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HL2T.Library.Models;

[Table("Sach")]
public class Sach
{
    [Key]
    public int MaSach { get; set; }

    [StringLength(20)]
    public string? ISBN { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên sách"), StringLength(255)]
    [Display(Name = "Tên sách")]
    public string TenSach { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập tác giả"), StringLength(150)]
    [Display(Name = "Tác giả")]
    public string TacGia { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "Nhà xuất bản")]
    public string? NhaXuatBan { get; set; }

    [Range(1900, 2100)]
    [Display(Name = "Năm xuất bản")]
    public int? NamXuatBan { get; set; }

    [Column(TypeName = "decimal(12,0)")]
    [Range(0, 100_000_000)]
    [Display(Name = "Giá bìa")]
    public decimal GiaBia { get; set; }

    [StringLength(255)]
    public string? AnhBia { get; set; }

    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Range(0, 100_000, ErrorMessage = "Số lượng không hợp lệ")]
    [Display(Name = "Tổng số lượng")]
    public int TongSoLuong { get; set; }

    [Display(Name = "Số lượng khả dụng")]
    public int SoLuongKhaDung { get; set; }

    [Display(Name = "Danh mục")]
    public int MaDanhMuc { get; set; }

    [ForeignKey(nameof(MaDanhMuc))]
    public DanhMucSach? DanhMucSach { get; set; }

    public ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

    public bool KiemTraKhaDung() => SoLuongKhaDung > 0;

    public void GiamSoLuong(int n)
    {
        if (n > SoLuongKhaDung) throw new InvalidOperationException($"Sách \"{TenSach}\" không đủ số lượng khả dụng.");
        SoLuongKhaDung -= n;
    }

    public void TangSoLuong(int n) => SoLuongKhaDung = Math.Min(TongSoLuong, SoLuongKhaDung + n);
}
