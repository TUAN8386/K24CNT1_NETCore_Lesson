using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATUAN_LESSON_10.Models
{
    [Table("Publisher")]
    public partial class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        public int PublisherId { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Nhà xuất bản")]
        public string PublisherName { get; set; } = null!;

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [InverseProperty("Publisher")]
        public virtual ICollection<Book> Books { get; set; }
    }
}