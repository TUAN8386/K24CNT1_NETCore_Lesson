using Microsoft.AspNetCore.Mvc;
using DAT_NetCoreMVC_LAB5.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DAT_NetCoreMVC_LAB5.Controllers
{
    public class DAT_AccountController : Controller
    {
        private static List<DAT_Account> accounts = new List<DAT_Account>
{
    new DAT_Account
    {
        Id = 1,
        FullName = "Nguyễn Văn A",
        Email = "vana@gmail.com",
        Phone = "0988422127",
        Address = "Hà Nội",
        Birthday = new DateTime(2000, 1, 1),
        Facebook = "https://facebook.com/vana"
    }
};
        // GET: DAT_Account/Index
        public IActionResult Index()
        {
            return View(accounts);
        }

        // GET: DAT_Account/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new DAT_Account();
            return View(model);
        }

        // POST: DAT_Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DAT_Account model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Id = accounts.Count + 1;
            accounts.Add(model);
            return RedirectToAction("Index");
        }

        // Remote Validation kiểm tra định dạng số điện thoại
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPhone(string phone)
        {
            string pattern = @"^(0[3|5|7|8|9])+([0-9]{8})$";
            if (!Regex.IsMatch(phone ?? "", pattern))
            {
                return Json("Số điện thoại không đúng định dạng (VD: 0988422127)");
            }

            return Json(true);
        }
    }
}