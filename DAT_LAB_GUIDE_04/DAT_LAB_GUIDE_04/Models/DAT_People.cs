using System;
using System.ComponentModel.DataAnnotations;

namespace DAT_LAB_GUIDE_04.Models
{
    public class DAT_People
    {
        [Display(Name = "Mã số")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Display(Name = "Địa chỉ email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ nơi ở")]
        public string Address { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Giới thiệu bản thân")]
        public string Bio { get; set; }

        [Display(Name = "Giới tính")]
        public byte Gender { get; set; } // 0: Cty, 1: Nam, 2: Nữ
    }
}