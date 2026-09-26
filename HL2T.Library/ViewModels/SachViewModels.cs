using System.ComponentModel.DataAnnotations;
using HL2T.Library.Models;

namespace HL2T.Library.ViewModels;

public class TimKiemSachVM
{
    public string? TuKhoa { get; set; }
    public int? MaDanhMuc { get; set; }
    public bool ChiConSach { get; set; }
    public int Trang { get; set; } = 1;
    public int KichThuocTrang { get; set; } = 10;
    public int TongSo { get; set; }
    public int TongSoTrang => (int)Math.Ceiling(TongSo / (double)KichThuocTrang);
    public List<Sach> KetQua { get; set; } = new();
    public List<DanhMucSach> DanhMucs { get; set; } = new();
}

public class SachFormVM
{
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

    [Range(1900, 2100, ErrorMessage = "Năm xuất bản không hợp lệ")]
    [Display(Name = "Năm xuất bản")]
    public int? NamXuatBan { get; set; }

    [Range(0, 100_000_000, ErrorMessage = "Giá bìa không hợp lệ")]
    [Display(Name = "Giá bìa (đ)")]
    public decimal GiaBia { get; set; }

    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Range(0, 100_000, ErrorMessage = "Tổng số lượng không hợp lệ")]
    [Display(Name = "Tổng số lượng")]
    public int TongSoLuong { get; set; } = 1;

    [Required(ErrorMessage = "Vui lòng chọn danh mục")]
    [Display(Name = "Danh mục")]
    public int MaDanhMuc { get; set; }

    public string? AnhBiaHienTai { get; set; }

    [Display(Name = "Ảnh bìa")]
    public IFormFile? AnhBia { get; set; }
}
