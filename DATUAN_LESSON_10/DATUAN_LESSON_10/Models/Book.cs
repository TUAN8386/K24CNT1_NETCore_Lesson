using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATUAN_LESSON_10.Models
{
    [Table("Book")]
    public partial class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(200)]
        [Display(Name = "Tên sách")]
        public string Title { get; set; } = null!;

        [StringLength(100)]
        [Display(Name = "Tác giả")]
        public string? Author { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Display(Name = "Năm xuất bản")]
        public int? PublishedYear { get; set; }

        [Display(Name = "Thể loại")]
        public int CategoryId { get; set; }

        [Display(Name = "Nhà xuất bản")]
        public int PublisherId { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Books")]
        public virtual Category? Category { get; set; }

        [ForeignKey("PublisherId")]
        [InverseProperty("Books")]
        public virtual Publisher? Publisher { get; set; }
    }
}