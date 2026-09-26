using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DatNetCoreLAB6_EF.Models
{
    [Table("DatSubjects")]
    public class DatSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên môn học không được để trống")]
        [StringLength(100, ErrorMessage = "Tên môn học giới hạn 100 ký tự")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên môn học")]
        public string SubjectName { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<DatMark> Marks { get; set; } = new List<DatMark>();
    }
}
