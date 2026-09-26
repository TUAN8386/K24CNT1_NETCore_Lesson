using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DinhAnhTuan2410900083_exam.Models
{
    [Table("DATStudent")]
    public class DATStudent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ và tên")]
        public string DATName { get; set; } = string.Empty;

        [Display(Name = "Giới tính (1: Nam, 0: Nữ)")]
        public byte DATGender { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DATBirthDay { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string DATEmail { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        public string? DATPhone { get; set; }

        [Display(Name = "Trạng thái")]
        public bool DATActive { get; set; }
    }
}