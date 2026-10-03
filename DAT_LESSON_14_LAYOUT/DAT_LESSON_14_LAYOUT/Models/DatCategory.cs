using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAT_LESSON_14_LAYOUT.Models
{
    [Table("DatCategory")]
    public class DatCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Trạng thái")]
        [Range(0, 1, ErrorMessage = "Trạng thái chỉ nhận 0 hoặc 1")]
        public byte Status { get; set; } = 1;

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; } = DateTime.Today;

        [Display(Name = "Ảnh")]
        [StringLength(100, ErrorMessage = "Đường dẫn ảnh tối đa 100 ký tự")]
        [Column(TypeName = "varchar(100)")]
        public string? Image { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(350, ErrorMessage = "Mô tả tối đa 350 ký tự")]
        [Column(TypeName = "nvarchar(350)")]
        public string? Description { get; set; }

        public ICollection<DatProduct> Products { get; set; } = new List<DatProduct>();
    }
}
