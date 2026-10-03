using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DAT_LESSON_14_LAYOUT.Models
{
    [Table("DatProduct")]
    public class DatProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Giá")]
        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public double? Price { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi phải lớn hơn hoặc bằng 0")]
        public double SalePrice { get; set; } = 0;

        [Display(Name = "Trạng thái")]
        [Range(0, 1, ErrorMessage = "Trạng thái chỉ nhận 0 hoặc 1")]
        public byte Status { get; set; } = 1;

        [Display(Name = "Danh mục")]
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int? CategoryId { get; set; }

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

        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public DatCategory? Category { get; set; }
    }
}
