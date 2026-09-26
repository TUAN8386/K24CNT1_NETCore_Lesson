using DatNetCoreLAB6_EF.Data;
using DatNetCoreLAB6_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Controllers
{
    // Bài tự làm 6: CRUD bảng DatStdClass
    public class DatStdClassesController : Controller
    {
        private readonly DatStudentDbContext _datContext;

        public DatStdClassesController(DatStudentDbContext datContext)
        {
            _datContext = datContext;
        }

        public async Task<IActionResult> DatIndex()
        {
            var datClasses = await _datContext.DatStdClasses
                .Include(datC => datC.Students)
                .OrderBy(datC => datC.ClassName)
                .ToListAsync();
            return View(datClasses);
        }

        public async Task<IActionResult> DatDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datClass = await _datContext.DatStdClasses
                .Include(datC => datC.Students)
                .FirstOrDefaultAsync(datC => datC.Id == id);
            if (datClass == null)
            {
                return NotFound();
            }
            return View(datClass);
        }

        public IActionResult DatCreate()
        {
            return View(new DatStdClass());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatCreate([Bind("ClassName")] DatStdClass datClass)
        {
            if (ModelState.IsValid)
            {
                _datContext.Add(datClass);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Thêm mới lớp thành công";
                return RedirectToAction(nameof(DatIndex));
            }
            return View(datClass);
        }

        public async Task<IActionResult> DatEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datClass = await _datContext.DatStdClasses.FindAsync(id);
            if (datClass == null)
            {
                return NotFound();
            }
            return View(datClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatEdit(int id, [Bind("Id,ClassName")] DatStdClass datClass)
        {
            if (id != datClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var datExisting = await _datContext.DatStdClasses.FindAsync(id);
                if (datExisting == null)
                {
                    return NotFound();
                }

                datExisting.ClassName = datClass.ClassName;
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Cập nhật lớp thành công";
                return RedirectToAction(nameof(DatIndex));
            }
            return View(datClass);
        }

        public async Task<IActionResult> DatDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datClass = await _datContext.DatStdClasses
                .Include(datC => datC.Students)
                .FirstOrDefaultAsync(datC => datC.Id == id);
            if (datClass == null)
            {
                return NotFound();
            }
            return View(datClass);
        }

        [HttpPost, ActionName("DatDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatDeleteConfirmed(int id)
        {
            var datClass = await _datContext.DatStdClasses.FindAsync(id);
            if (datClass != null)
            {
                if (await _datContext.DatStudents.AnyAsync(datS => datS.ClassId == id))
                {
                    TempData["DatError"] = "Không thể xóa: lớp đang có sinh viên";
                    return RedirectToAction(nameof(DatIndex));
                }

                _datContext.DatStdClasses.Remove(datClass);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Xóa lớp thành công";
            }
            return RedirectToAction(nameof(DatIndex));
        }
    }
}
