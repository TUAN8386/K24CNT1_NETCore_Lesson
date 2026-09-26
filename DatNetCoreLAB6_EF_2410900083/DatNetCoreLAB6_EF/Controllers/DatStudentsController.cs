using DatNetCoreLAB6_EF.Data;
using DatNetCoreLAB6_EF.Helpers;
using DatNetCoreLAB6_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Controllers
{
    // Bài tự làm 6: CRUD bảng DatStudent (có upload ảnh đại diện)
    public class DatStudentsController : Controller
    {
        private readonly DatStudentDbContext _datContext;
        private readonly IWebHostEnvironment _datEnvironment;

        public DatStudentsController(DatStudentDbContext datContext, IWebHostEnvironment datEnvironment)
        {
            _datContext = datContext;
            _datEnvironment = datEnvironment;
        }

        public async Task<IActionResult> DatIndex()
        {
            var datStudents = await _datContext.DatStudents
                .Include(datS => datS.StdClass)
                .OrderByDescending(datS => datS.Id)
                .ToListAsync();
            return View(datStudents);
        }

        public async Task<IActionResult> DatDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datStudent = await _datContext.DatStudents
                .Include(datS => datS.StdClass)
                .Include(datS => datS.Marks).ThenInclude(datM => datM.Subject)
                .FirstOrDefaultAsync(datS => datS.Id == id);
            if (datStudent == null)
            {
                return NotFound();
            }
            return View(datStudent);
        }

        public async Task<IActionResult> DatCreate()
        {
            await DatLoadClassesAsync(null);
            return View(new DatStudent());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatCreate(
            [Bind("StudentName,StudentEmail,StudentPhone,StudentAddress,StudentBirthday,ClassId")] DatStudent datStudent,
            IFormFile? datAvatarFile)
        {
            // Ảnh được upload qua file, nên bỏ lỗi Required của chuỗi StudentAvatar và tự kiểm tra
            ModelState.Remove(nameof(DatStudent.StudentAvatar));
            if (datAvatarFile == null || datAvatarFile.Length == 0)
            {
                ModelState.AddModelError(nameof(DatStudent.StudentAvatar), "Ảnh đại diện không được để trống");
            }
            else
            {
                var datImageError = DatFileHelper.DatValidateImage(datAvatarFile);
                if (datImageError != null)
                {
                    ModelState.AddModelError(nameof(DatStudent.StudentAvatar), datImageError);
                }
            }

            await DatValidateStudentAsync(datStudent);

            if (ModelState.IsValid)
            {
                datStudent.StudentAvatar = await DatFileHelper.DatSaveImageAsync(
                    datAvatarFile!, _datEnvironment.WebRootPath, DatFileHelper.DAT_AVATAR_FOLDER);
                _datContext.Add(datStudent);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Thêm mới sinh viên thành công";
                return RedirectToAction(nameof(DatIndex));
            }

            await DatLoadClassesAsync(datStudent.ClassId);
            return View(datStudent);
        }

        public async Task<IActionResult> DatEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datStudent = await _datContext.DatStudents.FindAsync(id);
            if (datStudent == null)
            {
                return NotFound();
            }

            await DatLoadClassesAsync(datStudent.ClassId);
            return View(datStudent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatEdit(int id,
            [Bind("Id,StudentName,StudentEmail,StudentPhone,StudentAddress,StudentAvatar,StudentBirthday,ClassId")] DatStudent datStudent,
            IFormFile? datAvatarFile)
        {
            if (id != datStudent.Id)
            {
                return NotFound();
            }

            // Sửa: ảnh không bắt buộc chọn lại (giữ ảnh cũ)
            ModelState.Remove(nameof(DatStudent.StudentAvatar));
            var datImageError = DatFileHelper.DatValidateImage(datAvatarFile);
            if (datImageError != null)
            {
                ModelState.AddModelError(nameof(DatStudent.StudentAvatar), datImageError);
            }

            await DatValidateStudentAsync(datStudent);

            if (ModelState.IsValid)
            {
                var datExisting = await _datContext.DatStudents.FindAsync(id);
                if (datExisting == null)
                {
                    return NotFound();
                }

                if (datAvatarFile != null && datAvatarFile.Length > 0)
                {
                    var datNewAvatar = await DatFileHelper.DatSaveImageAsync(
                        datAvatarFile, _datEnvironment.WebRootPath, DatFileHelper.DAT_AVATAR_FOLDER);
                    DatFileHelper.DatDeleteImage(datExisting.StudentAvatar, _datEnvironment.WebRootPath, DatFileHelper.DAT_AVATAR_FOLDER);
                    datExisting.StudentAvatar = datNewAvatar;
                }

                datExisting.StudentName = datStudent.StudentName;
                datExisting.StudentEmail = datStudent.StudentEmail;
                datExisting.StudentPhone = datStudent.StudentPhone;
                datExisting.StudentAddress = datStudent.StudentAddress;
                datExisting.StudentBirthday = datStudent.StudentBirthday;
                datExisting.ClassId = datStudent.ClassId;

                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Cập nhật sinh viên thành công";
                return RedirectToAction(nameof(DatIndex));
            }

            await DatLoadClassesAsync(datStudent.ClassId);
            return View(datStudent);
        }

        public async Task<IActionResult> DatDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datStudent = await _datContext.DatStudents
                .Include(datS => datS.StdClass)
                .FirstOrDefaultAsync(datS => datS.Id == id);
            if (datStudent == null)
            {
                return NotFound();
            }
            return View(datStudent);
        }

        [HttpPost, ActionName("DatDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatDeleteConfirmed(int id)
        {
            var datStudent = await _datContext.DatStudents.FindAsync(id);
            if (datStudent != null)
            {
                DatFileHelper.DatDeleteImage(datStudent.StudentAvatar, _datEnvironment.WebRootPath, DatFileHelper.DAT_AVATAR_FOLDER);
                _datContext.DatStudents.Remove(datStudent); // điểm của sinh viên bị xóa theo (cascade)
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Xóa sinh viên thành công";
            }
            return RedirectToAction(nameof(DatIndex));
        }

        private async Task DatValidateStudentAsync(DatStudent datStudent)
        {
            if (!string.IsNullOrWhiteSpace(datStudent.StudentEmail) &&
                await _datContext.DatStudents.AnyAsync(datS => datS.StudentEmail == datStudent.StudentEmail && datS.Id != datStudent.Id))
            {
                ModelState.AddModelError(nameof(DatStudent.StudentEmail), "Email đã tồn tại");
            }

            if (!string.IsNullOrWhiteSpace(datStudent.StudentPhone) &&
                await _datContext.DatStudents.AnyAsync(datS => datS.StudentPhone == datStudent.StudentPhone && datS.Id != datStudent.Id))
            {
                ModelState.AddModelError(nameof(DatStudent.StudentPhone), "Số điện thoại đã tồn tại");
            }

            if (datStudent.StudentBirthday.HasValue && datStudent.StudentBirthday.Value.Date >= DateTime.Today)
            {
                ModelState.AddModelError(nameof(DatStudent.StudentBirthday), "Ngày sinh phải nhỏ hơn ngày hiện tại");
            }
        }

        private async Task DatLoadClassesAsync(int? datSelectedId)
        {
            var datClasses = await _datContext.DatStdClasses.OrderBy(datC => datC.ClassName).ToListAsync();
            ViewData["ClassId"] = new SelectList(datClasses, "Id", "ClassName", datSelectedId);
        }
    }
}
