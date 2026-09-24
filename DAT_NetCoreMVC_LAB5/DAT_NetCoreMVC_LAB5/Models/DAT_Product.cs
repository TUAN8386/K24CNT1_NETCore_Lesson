using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DAT_NetCoreMVC_LAB5.Attributes;

namespace DAT_NetCoreMVC_LAB5.Models
{
    public class DAT_Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "Tên sản phẩm phải từ 6 đến 150 ký tự")]
        public string Name { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Display(Name = "Chọn ảnh upload")]
        [Required(ErrorMessage = "Vui lòng chọn ảnh upload")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Giá chuẩn")]
        [Required(ErrorMessage = "Giá chuẩn không được để trống")]
        [Range(100000, double.MaxValue, ErrorMessage = "Giá chuẩn phải từ 100,000 VNĐ trở lên")]
        public float Price { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        [Required(ErrorMessage = "Giá khuyến mãi không được để trống")]
        [DAT_SalePrice]
        public float SalePrice { get; set; }

        [Display(Name = "Danh mục")]
        [Required(ErrorMessage = "Vui lòng chọn danh mục sản phẩm")]
        public int CategoryId { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống")]
        [StringLength(1500, ErrorMessage = "Mô tả không vượt quá 1500 ký tự")]
        [DAT_BadWord]
        public string Description { get; set; }
    }
}