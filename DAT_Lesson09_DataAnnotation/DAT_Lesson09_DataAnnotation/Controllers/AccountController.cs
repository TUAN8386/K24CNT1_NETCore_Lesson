using Microsoft.AspNetCore.Mvc;
using DAT_Lesson09_DataAnnotation.Models;
using System.Collections.Generic;
using System.Linq;

namespace DAT_Lesson09_DataAnnotation.Controllers
{
    public class AccountController : Controller
    {
        // Danh sách tĩnh chứa 3 bản ghi mẫu trong bộ nhớ
        private static List<RegisterViewModel> accounts = new List<RegisterViewModel>
        {
            new RegisterViewModel
            {
                Id = 1,
                Username = "DINH ANH TUAN",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Email = "Datuan@gmail.com",
                Phone = "0982359233",
                Age = 22
            },
            new RegisterViewModel
            {
                Id = 2,
                Username = "tranthib",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Email = "thib@gmail.com",
                Phone = "0912345678",
                Age = 25
            },
            new RegisterViewModel
            {
                Id = 3,
                Username = "levanc",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Email = "vanc@gmail.com",
                Phone = "0987654321",
                Age = 30
            }
        };

        // GET: Account/Index (Hiển thị danh sách)
        [HttpGet]
        public IActionResult Index()
        {
            return View(accounts);
        }

        // GET: Account/Register (Hiển thị Form)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register (Xử lý Đăng ký)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            // Kiểm tra Validation dữ liệu
            if (!ModelState.IsValid)
            {
                // Nếu có lỗi -> Trả về View kèm thông báo lỗi
                return View(model);
            }

            // Tự tăng ID cho bản ghi mới
            model.Id = accounts.Count > 0 ? accounts.Max(a => a.Id) + 1 : 1;

            // Lưu vào danh sách mẫu
            accounts.Add(model);

            // Chuyển hướng về trang danh sách
            return RedirectToAction("Index");
        }
    }
}