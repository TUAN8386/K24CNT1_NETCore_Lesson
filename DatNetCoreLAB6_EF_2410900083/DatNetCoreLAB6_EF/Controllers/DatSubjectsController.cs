using DatNetCoreLAB6_EF.Data;
using DatNetCoreLAB6_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Controllers
{
    // Bài tự làm 6: CRUD bảng DatSubjects
    public class DatSubjectsController : Controller
    {
        private readonly DatStudentDbContext _datContext;

        public DatSubjectsController(DatStudentDbContext datContext)
        {
            _datContext = datContext;
        }

        public async Task<IActionResult> DatIndex()
        {
            var datSubjects = await _datContext.DatSubjects
                .OrderBy(datS => datS.SubjectName)
                .ToListAsync();
            return View(datSubjects);
        }

        public async Task<IActionResult> DatDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datSubject = await _datContext.DatSubjects
                .Include(datS => datS.Marks).ThenInclude(datM => datM.Student)
                .FirstOrDefaultAsync(datS => datS.Id == id);
            if (datSubject == null)
            {
                return NotFound();
            }
            return View(datSubject);
        }

        public IActionResult DatCreate()
        {
            return View(new DatSubject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatCreate([Bind("SubjectName")] DatSubject datSubject)
        {
            await DatCheckUniqueAsync(datSubject);

            if (ModelState.IsValid)
            {
                _datContext.Add(datSubject);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Thêm mới môn học thành công";
                return RedirectToAction(nameof(DatIndex));
            }
            return View(datSubject);
        }

        public async Task<IActionResult> DatEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datSubject = await _datContext.DatSubjects.FindAsync(id);
            if (datSubject == null)
            {
                return NotFound();
            }
            return View(datSubject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatEdit(int id, [Bind("Id,SubjectName")] DatSubject datSubject)
        {
            if (id != datSubject.Id)
            {
                return NotFound();
            }

            await DatCheckUniqueAsync(datSubject);

            if (ModelState.IsValid)
            {
                var datExisting = await _datContext.DatSubjects.FindAsync(id);
                if (datExisting == null)
                {
                    return NotFound();
                }

                datExisting.SubjectName = datSubject.SubjectName;
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Cập nhật môn học thành công";
                return RedirectToAction(nameof(DatIndex));
            }
            return View(datSubject);
        }

        public async Task<IActionResult> DatDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datSubject = await _datContext.DatSubjects.FirstOrDefaultAsync(datS => datS.Id == id);
            if (datSubject == null)
            {
                return NotFound();
            }
            return View(datSubject);
        }

        [HttpPost, ActionName("DatDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatDeleteConfirmed(int id)
        {
            var datSubject = await _datContext.DatSubjects.FindAsync(id);
            if (datSubject != null)
            {
                if (await _datContext.DatMarks.AnyAsync(datM => datM.SubjectId == id))
                {
                    TempData["DatError"] = "Không thể xóa: môn học đã có điểm";
                    return RedirectToAction(nameof(DatIndex));
                }

                _datContext.DatSubjects.Remove(datSubject);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Xóa môn học thành công";
            }
            return RedirectToAction(nameof(DatIndex));
        }

        private async Task DatCheckUniqueAsync(DatSubject datSubject)
        {
            if (!string.IsNullOrWhiteSpace(datSubject.SubjectName) &&
                await _datContext.DatSubjects.AnyAsync(datS => datS.SubjectName == datSubject.SubjectName && datS.Id != datSubject.Id))
            {
                ModelState.AddModelError(nameof(DatSubject.SubjectName), "Tên môn học đã tồn tại");
            }
        }
    }
}
