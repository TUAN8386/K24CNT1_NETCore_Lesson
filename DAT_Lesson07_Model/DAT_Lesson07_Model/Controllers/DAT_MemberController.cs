using Microsoft.AspNetCore.Mvc;
using DAT_Lesson07_Model.DataModel;

namespace DAT_Lesson07_Model.Controllers
{
    public class DAT_MemberController : Controller
    {
        // Khai báo danh sách static lưu dữ liệu trong bộ nhớ
        protected static List<DAT_Member> memberList = new List<DAT_Member>()
        {
            new DAT_Member(Guid.NewGuid().ToString(), "datuser", "123", "ĐINH ANH TUẤN", "dat02376@gmail.com"),
            new DAT_Member(Guid.NewGuid().ToString(), "user1", "123", "Nguyễn Văn A", "nva@gmail.com"),
            new DAT_Member(Guid.NewGuid().ToString(), "user2", "123", "Trần Thị B", "ttb@gmail.com")
        };

        // 1. Action hiển thị danh sách (Strong Typing)
        public IActionResult Index()
        {
            return View(memberList);
        }

        // 2. Action trả về giao diện thêm mới (GET)
        public IActionResult CreateMember()
        {
            return View();
        }

        // 3. Action xử lý nhận dữ liệu từ Form (POST)
        [HttpPost]
        public IActionResult CreateMember(DAT_Member member)
        {
            // Tự động sinh GUID cho ID
            member.DAT_MemberID = Guid.NewGuid().ToString();

            // Thêm đối tượng vào danh sách
            memberList.Add(member);

            // Quay về trang danh sách
            return RedirectToAction("Index");
        }
    }
}