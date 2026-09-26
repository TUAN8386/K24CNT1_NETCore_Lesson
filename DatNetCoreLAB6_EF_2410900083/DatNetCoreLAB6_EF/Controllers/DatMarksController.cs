using DatNetCoreLAB6_EF.Data;
using DatNetCoreLAB6_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Controllers
{
    // Bài tự làm 6: CRUD bảng DatMarks (khóa chính 2 cột SubjectId, StudentId)
    public class DatMarksController : Controller
    {
        private readonly DatStudentDbContext _datContext;

        public DatMarksController(DatStudentDbContext datContext)
        {
            _datContext = datContext;
        }

        public async Task<IActionResult> DatIndex()
        {
            var datMarks = await _datContext.DatMarks
                .Include(datM => datM.Student)
                .Include(datM => datM.Subject)
                .OrderBy(datM => datM.Student!.StudentName)
                .ThenBy(datM => datM.Subject!.SubjectName)
                .ToListAsync();
            return View(datMarks);
        }

        public async Task<IActionResult> DatDetails(int? subjectId, int? studentId)
        {
            var datMark = await DatFindMarkAsync(subjectId, studentId);
            if (datMark == null)
            {
                return NotFound();
            }
            return View(datMark);
        }

        public async Task<IActionResult> DatCreate()
        {
            await DatLoadSelectsAsync(null, null);
            return View(new DatMark());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatCreate([Bind("SubjectId,StudentId,Score")] DatMark datMark)
        {
            if (await _datContext.DatMarks.AnyAsync(datM => datM.SubjectId == datMark.SubjectId && datM.StudentId == datMark.StudentId))
            {
                ModelState.AddModelError(string.Empty, "Sinh viên này đã có điểm môn học này");
            }

            if (ModelState.IsValid)
            {
                _datContext.Add(datMark);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Thêm mới điểm thành công";
                return RedirectToAction(nameof(DatIndex));
            }

            await DatLoadSelectsAsync(datMark.SubjectId, datMark.StudentId);
            return View(datMark);
        }

        public async Task<IActionResult> DatEdit(int? subjectId, int? studentId)
        {
            var datMark = await DatFindMarkAsync(subjectId, studentId);
            if (datMark == null)
            {
                return NotFound();
            }
            return View(datMark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatEdit(int subjectId, int studentId, [Bind("SubjectId,StudentId,Score")] DatMark datMark)
        {
            if (subjectId != datMark.SubjectId || studentId != datMark.StudentId)
            {
                return NotFound();
            }

            var datExisting = await DatFindMarkAsync(subjectId, studentId);
            if (datExisting == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                datExisting.Score = datMark.Score;
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Cập nhật điểm thành công";
                return RedirectToAction(nameof(DatIndex));
            }

            // Hiển thị lại tên sinh viên / môn học trên form
            datMark.Student = datExisting.Student;
            datMark.Subject = datExisting.Subject;
            return View(datMark);
        }

        public async Task<IActionResult> DatDelete(int? subjectId, int? studentId)
        {
            var datMark = await DatFindMarkAsync(subjectId, studentId);
            if (datMark == null)
            {
                return NotFound();
            }
            return View(datMark);
        }

        [HttpPost, ActionName("DatDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatDeleteConfirmed(int subjectId, int studentId)
        {
            var datMark = await _datContext.DatMarks.FindAsync(subjectId, studentId);
            if (datMark != null)
            {
                _datContext.DatMarks.Remove(datMark);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Xóa điểm thành công";
            }
            return RedirectToAction(nameof(DatIndex));
        }

        private async Task<DatMark?> DatFindMarkAsync(int? datSubjectId, int? datStudentId)
        {
            if (datSubjectId == null || datStudentId == null)
            {
                return null;
            }

            return await _datContext.DatMarks
                .Include(datM => datM.Student)
                .Include(datM => datM.Subject)
                .FirstOrDefaultAsync(datM => datM.SubjectId == datSubjectId && datM.StudentId == datStudentId);
        }

        private async Task DatLoadSelectsAsync(int? datSubjectId, int? datStudentId)
        {
            var datSubjects = await _datContext.DatSubjects.OrderBy(datS => datS.SubjectName).ToListAsync();
            var datStudents = await _datContext.DatStudents.OrderBy(datS => datS.StudentName).ToListAsync();
            ViewData["SubjectId"] = new SelectList(datSubjects, "Id", "SubjectName", datSubjectId);
            ViewData["StudentId"] = new SelectList(datStudents, "Id", "StudentName", datStudentId);
        }
    }
}
