using Microsoft.AspNetCore.Mvc;
using DAT_Lesson04_Lab.Models;

namespace DAT_Lesson04_Lab.Controllers
{
    public class DATAccountController : Controller
    {
        // Khởi tạo danh sách mốc dữ liệu giả (Mock Data)
        private static List<DATAccount> listAccount = new List<DATAccount>()
        {
            new DATAccount()
            {
                Id = 1,
                Name = "Nguyễn Văn An",
                Email = "an.nguyen@gmail.com",
                Phone = "0987654321",
                Avatar = "/images/4.jpg",
                Address = "Hà Nội",
                Bio = "Lập trình viên backend .NET",
                Gender = 1,
                Birthday = new DateTime(2002, 05, 20)
            },
            new DATAccount()
            {
                Id = 2,
                Name = "Trần Thị Bích",
                Email = "bich.tran@gmail.com",
                Phone = "0912345678",
                Avatar = "/images/2.jpg",
                Address = "Hải Phòng",
                Bio = "Chuyên viên thiết kế UI/UX",
                Gender = 0,
                Birthday = new DateTime(2003, 08, 15)
            },
            new DATAccount()
            {
                Id = 3,
                Name = "Hoàng Quốc Dũng",
                Email = "dung.hoang@gmail.com",
                Phone = "0933445566",
                Avatar = "/images/3.jpg",
                Address = "Đà Nẵng",
                Bio = "Quản trị hệ thống mạng",
                Gender = 1,
                Birthday = new DateTime(2001, 11, 02)
            },
            new DATAccount()
            {
                Id = 4,
                Name = "Lê Mai Anh",
                Email = "anh.le@gmail.com",
                Phone = "0977889900",
                Avatar = "/images/1.jpg",
                Address = "Cần Thơ",
                Bio = "Chuyên viên phân tích dữ liệu (Data Analyst)",
                Gender = 0,
                Birthday = new DateTime(2004, 03, 10)
            },
            // === THÊM BẢN GHI THỨ 5 ===
            new DATAccount()
            {
                Id = 5,
                Name = "Phạm Đăng Khoa",
                Email = "khoa.pham@gmail.com",
                Phone = "0944556677",
                Avatar = "/images/5.jpg",
                Address = "TP. Hồ Chí Minh",
                Bio = "Kỹ sư kiểm thử phần mềm (QA/QC)",
                Gender = 1,
                Birthday = new DateTime(2000, 12, 25)
            }
        };

        // 1. Action hiển thị danh sách tất cả tài khoản
        public IActionResult DATIndex()
        {
            ViewBag.DATAccounts = listAccount;
            return View();
        }

        // 2. Action hiển thị chi tiết hồ sơ theo ID
        public IActionResult DATProfile(int? id)
        {
            DATAccount account;
            if (id == null)
            {
                // Nếu không truyền ID thì lấy tài khoản đầu tiên
                account = listAccount.FirstOrDefault();
            }
            else
            {
                // Lọc ra tài khoản khớp với ID truyền vào
                account = listAccount.FirstOrDefault(a => a.Id == id);
            }

            ViewBag.DATAccount = account;
            return View();
        }
    }
}