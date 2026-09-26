using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DatNetCoreLAB6_EF.Models
{
    // Khóa chính trên 2 cột (SubjectId, StudentId) - cấu hình trong DatStudentDbContext
    [Table("DatMarks")]
    public class DatMark
    {
        [Required(ErrorMessage = "Môn học không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn môn học")]
        [Display(Name = "Môn học")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Sinh viên không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn sinh viên")]
        [Display(Name = "Sinh viên")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Điểm không được để trống")]
        [Range(0, 10, ErrorMessage = "Điểm phải nằm trong khoảng 0 - 10")]
        [Column(TypeName = "float")]
        [Display(Name = "Điểm")]
        public double? Score { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(SubjectId))]
        public DatSubject? Subject { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(StudentId))]
        public DatStudent? Student { get; set; }
    }
}
