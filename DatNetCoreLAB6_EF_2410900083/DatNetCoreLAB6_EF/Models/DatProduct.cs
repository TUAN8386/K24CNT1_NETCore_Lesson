using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DatNetCoreLAB6_EF.Models
{
    [Table("DatProduct")]
    public class DatProduct
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150, ErrorMessage = "Tên sản phẩm giới hạn 150 ký tự")]
        [Column(TypeName = "nvarchar(150)")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(150)")]
        [StringLength(150)]
        [Display(Name = "Ảnh")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải >= 0")]
        [Display(Name = "Giá")]
        public float Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi phải >= 0")]
        [Display(Name = "Giá KM")]
        public float SalePrice { get; set; }

        [Column(TypeName = "tinyint")]
        [Range(0, 1, ErrorMessage = "Trạng thái chỉ nhận 0 hoặc 1")]
        [Display(Name = "Trạng thái")]
        public byte Status { get; set; } = 1;

        [StringLength(1000, ErrorMessage = "Nội dung mô tả giới hạn 1000 ký tự")]
        [Column(TypeName = "ntext")]
        [Display(Name = "Mô tả")]
        public string? Descriptions { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục sản phẩm")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreatedDate { get; set; }

        // Khóa ngoại tới bảng DatCategory
        [ValidateNever]
        [ForeignKey(nameof(CategoryId))]
        public DatCategory? Category { get; set; }
    }
}
