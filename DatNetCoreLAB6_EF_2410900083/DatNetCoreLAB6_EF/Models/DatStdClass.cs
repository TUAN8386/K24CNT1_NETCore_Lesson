using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DatNetCoreLAB6_EF.Models
{
    [Table("DatStdClass")]
    public class DatStdClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống")]
        [StringLength(100, ErrorMessage = "Tên lớp giới hạn 100 ký tự")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên lớp")]
        public string ClassName { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<DatStudent> Students { get; set; } = new List<DatStudent>();
    }
}
