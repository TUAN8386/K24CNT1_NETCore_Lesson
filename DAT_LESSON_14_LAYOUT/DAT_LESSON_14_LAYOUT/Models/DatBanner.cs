using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAT_LESSON_14_LAYOUT.Models
{
    [Table("DatBanner")]
    public class DatBanner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Tên banner")]
        [Required(ErrorMessage = "Tên banner không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Trạng thái")]
        [Range(0, 1, ErrorMessage = "Trạng thái chỉ nhận 0 hoặc 1")]
        public byte Status { get; set; } = 1;

        // Cột "Prioty" trong đề bài - độ ưu tiên hiển thị
        [Display(Name = "Độ ưu tiên")]
        [Range(0, int.MaxValue, ErrorMessage = "Độ ưu tiên phải >= 0")]
        public int Priority { get; set; } = 0;

        [Display(Name = "Ảnh")]
        [StringLength(100, ErrorMessage = "Đường dẫn ảnh tối đa 100 ký tự")]
        [Column(TypeName = "varchar(100)")]
        public string? Image { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(350, ErrorMessage = "Mô tả tối đa 350 ký tự")]
        [Column(TypeName = "nvarchar(350)")]
        public string? Description { get; set; }
    }
}
