using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HL2T.Library.Models;

[Table("VaiTro")]
public class VaiTro
{
    [Key]
    public int MaVaiTro { get; set; }

    [Required, StringLength(50)]
    public string TenVaiTro { get; set; } = string.Empty;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}

/// <summary>Tên vai trò dùng cho phân quyền [Authorize(Roles = ...)].</summary>
public static class VaiTroNames
{
    public const string Admin = "Admin";
    public const string ThuThu = "ThuThu";
    public const string DocGia = "DocGia";
    public const string QuanTri = Admin + "," + ThuThu;

    public static string HienThi(string? ten) => ten switch
    {
        Admin => "Quản trị viên",
        ThuThu => "Thủ thư",
        DocGia => "Độc giả",
        _ => ten ?? ""
    };
}
