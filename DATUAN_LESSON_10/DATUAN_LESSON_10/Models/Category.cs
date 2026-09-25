using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATUAN_LESSON_10.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên thể loại")]
        public string CategoryName { get; set; } = null!;

        [StringLength(255)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Book> Books { get; set; }
    }
}