using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DatNetCoreLAB6_EF.Models
{
    [Table("DatStudent")]
    public class DatStudent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên giới hạn 100 ký tự")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Họ tên")]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(100, ErrorMessage = "Email giới hạn 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Email")]
        public string StudentEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(50, ErrorMessage = "Số điện thoại giới hạn 50 ký tự")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại gồm 10-11 chữ số, bắt đầu bằng 0")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Điện thoại")]
        public string StudentPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(150, ErrorMessage = "Địa chỉ giới hạn 150 ký tự")]
        [Column(TypeName = "nvarchar(150)")]
        [Display(Name = "Địa chỉ")]
        public string StudentAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ảnh đại diện không được để trống")]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Ảnh đại diện")]
        public string StudentAvatar { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày sinh")]
        public DateTime? StudentBirthday { get; set; }

        [Required(ErrorMessage = "Lớp không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lớp")]
        [Display(Name = "Lớp")]
        public int ClassId { get; set; }

        // Khóa ngoại tới bảng DatStdClass
        [ValidateNever]
        [ForeignKey(nameof(ClassId))]
        public DatStdClass? StdClass { get; set; }

        [ValidateNever]
        public ICollection<DatMark> Marks { get; set; } = new List<DatMark>();
    }
}
