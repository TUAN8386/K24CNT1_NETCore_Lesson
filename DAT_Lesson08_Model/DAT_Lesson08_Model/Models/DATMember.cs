using System.ComponentModel.DataAnnotations;

namespace DAT_Lesson08_Model.Models
{
    public class DATMember
    {
        public string DATMemberID { get; set; }

        [Display(Name = "Tên tài khoản")]
        public string DATUsername { get; set; }

        [Display(Name = "Mật khẩu")]
        public string DATPassword { get; set; }

        [Display(Name = "Họ và tên")]
        public string DATFullName { get; set; }

        [Display(Name = "Địa chỉ Email")]
        public string DATEmail { get; set; }
    }
}