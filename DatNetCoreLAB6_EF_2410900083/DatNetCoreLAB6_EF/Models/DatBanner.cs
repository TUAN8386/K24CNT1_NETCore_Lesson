using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatNetCoreLAB6_EF.Models
{
    [Table("DatBanner")]
    public class DatBanner
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên banner không được để trống")]
        [StringLength(150, ErrorMessage = "Tên banner giới hạn 150 ký tự")]
        [Column(TypeName = "nvarchar(150)")]
        [Display(Name = "Tên banner")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(150)")]
        [StringLength(150)]
        [Display(Name = "Ảnh")]
        public string? Image { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả giới hạn 500 ký tự")]
        [Column(TypeName = "nvarchar(500)")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "tinyint")]
        [Range(0, 1, ErrorMessage = "Trạng thái chỉ nhận 0 hoặc 1")]
        [Display(Name = "Trạng thái")]
        public byte Status { get; set; } = 1;
    }
}
