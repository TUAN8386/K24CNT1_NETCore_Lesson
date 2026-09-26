using System.ComponentModel.DataAnnotations;
using HL2T.Library.Models;

namespace HL2T.Library.ViewModels;

public class LapPhieuVM
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập hoặc email độc giả")]
    [Display(Name = "Độc giả (tên đăng nhập / email)")]
    public string DocGia { get; set; } = string.Empty;

    [Display(Name = "Mã sách (cách nhau bởi dấu phẩy)")]
    [Required(ErrorMessage = "Vui lòng nhập mã sách")]
    public string DanhSachMaSach { get; set; } = string.Empty;
}

public class TraSachDongVM
{
    public int MaChiTiet { get; set; }
    public string TenSach { get; set; } = string.Empty;
    public decimal GiaBia { get; set; }
    public int SoLuong { get; set; }
    public bool ChonTra { get; set; } = true;
    public string TinhTrang { get; set; } = TinhTrangSach.BinhThuong;
}

public class TraSachVM
{
    public int MaPhieuMuon { get; set; }
    public PhieuMuon? Phieu { get; set; }
    public List<TraSachDongVM> Dong { get; set; } = new();
}

public class TienPhatDuKien
{
    public int SoNgayTre { get; set; }
    public decimal PhatTreHan { get; set; }
    public decimal BoiThuong { get; set; }
    public decimal Tong => PhatTreHan + BoiThuong;
}
