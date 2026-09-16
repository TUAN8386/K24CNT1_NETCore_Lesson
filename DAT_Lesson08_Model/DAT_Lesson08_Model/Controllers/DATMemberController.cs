using Microsoft.AspNetCore.Mvc;
using DAT_Lesson08_Model.Models;

namespace DAT_Lesson08_Model.Controllers
{
    public class DATMemberController : Controller
    {
        // Danh sách mốc dữ liệu mẫu (Mock data static)
        private static List<DATMember> members = new List<DATMember>
        {
            new DATMember { DATMemberID = Guid.NewGuid().ToString(), DATUsername = "user01", DATPassword = "123", DATFullName = "ĐINH ANH TUẤ", DATEmail = "dat01@gmail.com" },
            new DATMember { DATMemberID = Guid.NewGuid().ToString(), DATUsername = "user02", DATPassword = "123", DATFullName = "Trần Thị B", DATEmail = "b@gmail.com" },
            new DATMember { DATMemberID = Guid.NewGuid().ToString(), DATUsername = "user03", DATPassword = "123", DATFullName = "Lê Văn Cường", DATEmail = "cuong@gmail.com" },
            new DATMember { DATMemberID = Guid.NewGuid().ToString(), DATUsername = "user04", DATPassword = "123", DATFullName = "Phạm Hoàng D", DATEmail = "d@gmail.com" },
            new DATMember { DATMemberID = Guid.NewGuid().ToString(), DATUsername = "user05", DATPassword = "123", DATFullName = "Hoàng Thị E", DATEmail = "e@gmail.com" }
        };

        // 1. Trang danh sách (Index)
        public IActionResult Index()
        {
            return View(members);
        }

        // 2. Thêm mới - GET
        public IActionResult DATCreate()
        {
            return View();
        }

        // 2. Thêm mới - POST
        [HttpPost]
        public IActionResult DATCreate(DATMember member)
        {
            member.DATMemberID = Guid.NewGuid().ToString(); // Tự sinh ID
            members.Add(member);
            return RedirectToAction(nameof(Index));
        }

        // 3. Sửa - GET
        public IActionResult DATEdit(string id)
        {
            var item = members.FirstOrDefault(x => x.DATMemberID == id);
            return View(item);
        }

        // 3. Sửa - POST
        [HttpPost]
        public IActionResult DATEdit(string id, DATMember member)
        {
            var item = members.FirstOrDefault(x => x.DATMemberID == id);
            if (item != null)
            {
                item.DATUsername = member.DATUsername;
                item.DATPassword = member.DATPassword;
                item.DATFullName = member.DATFullName;
                item.DATEmail = member.DATEmail;
            }
            return RedirectToAction(nameof(Index));
        }

        // 4. Xem chi tiết - GET
        public IActionResult DATDetail(string id)
        {
            var item = members.FirstOrDefault(x => x.DATMemberID == id);
            return View(item);
        }

        // 5. Xóa - GET
        public IActionResult DATDelete(string id)
        {
            var item = members.FirstOrDefault(x => x.DATMemberID == id);
            return View(item);
        }

        // 5. Xóa - POST
        [HttpPost]
        public IActionResult DATDeleted(string id)
        {
            var item = members.FirstOrDefault(x => x.DATMemberID == id);
            if (item != null)
            {
                members.Remove(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}