using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HL2T.Library.Models;

[Table("DanhMucSach")]
public class DanhMucSach
{
    [Key]
    public int MaDanhMuc { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên danh mục"), StringLength(100)]
    [Display(Name = "Tên danh mục")]
    public string TenDanhMuc { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    public ICollection<Sach> Sachs { get; set; } = new List<Sach>();
}
