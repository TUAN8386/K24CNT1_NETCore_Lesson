using DatNetCoreLAB6_EF.Data;
using DatNetCoreLAB6_EF.Helpers;
using DatNetCoreLAB6_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Controllers
{
    // Bài tự làm 3: CRUD bảng DatBanner
    public class DatBannersController : Controller
    {
        private readonly DatAppDbContext _datContext;
        private readonly IWebHostEnvironment _datEnvironment;

        public DatBannersController(DatAppDbContext datContext, IWebHostEnvironment datEnvironment)
        {
            _datContext = datContext;
            _datEnvironment = datEnvironment;
        }

        public async Task<IActionResult> DatIndex()
        {
            var datBanners = await _datContext.DatBanners
                .OrderByDescending(datB => datB.Id)
                .ToListAsync();
            return View(datBanners);
        }

        public async Task<IActionResult> DatDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datBanner = await _datContext.DatBanners.FirstOrDefaultAsync(datB => datB.Id == id);
            if (datBanner == null)
            {
                return NotFound();
            }
            return View(datBanner);
        }

        public IActionResult DatCreate()
        {
            return View(new DatBanner());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatCreate([Bind("Name,Description,Status")] DatBanner datBanner, IFormFile? datImageFile)
        {
            if (datImageFile == null || datImageFile.Length == 0)
            {
                ModelState.AddModelError(nameof(DatBanner.Image), "Vui lòng chọn ảnh banner");
            }
            else
            {
                var datImageError = DatFileHelper.DatValidateImage(datImageFile);
                if (datImageError != null)
                {
                    ModelState.AddModelError(nameof(DatBanner.Image), datImageError);
                }
            }

            if (ModelState.IsValid)
            {
                datBanner.Image = await DatFileHelper.DatSaveImageAsync(
                    datImageFile!, _datEnvironment.WebRootPath, DatFileHelper.DAT_BANNER_FOLDER);
                datBanner.CreatedDate = DateTime.Now;
                _datContext.Add(datBanner);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Thêm mới banner thành công";
                return RedirectToAction(nameof(DatIndex));
            }
            return View(datBanner);
        }

        public async Task<IActionResult> DatEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datBanner = await _datContext.DatBanners.FindAsync(id);
            if (datBanner == null)
            {
                return NotFound();
            }
            return View(datBanner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatEdit(int id, [Bind("Id,Name,Image,Description,Status")] DatBanner datBanner, IFormFile? datImageFile)
        {
            if (id != datBanner.Id)
            {
                return NotFound();
            }

            var datImageError = DatFileHelper.DatValidateImage(datImageFile);
            if (datImageError != null)
            {
                ModelState.AddModelError(nameof(DatBanner.Image), datImageError);
            }

            if (ModelState.IsValid)
            {
                var datExisting = await _datContext.DatBanners.FindAsync(id);
                if (datExisting == null)
                {
                    return NotFound();
                }

                if (datImageFile != null && datImageFile.Length > 0)
                {
                    var datNewImage = await DatFileHelper.DatSaveImageAsync(
                        datImageFile, _datEnvironment.WebRootPath, DatFileHelper.DAT_BANNER_FOLDER);
                    DatFileHelper.DatDeleteImage(datExisting.Image, _datEnvironment.WebRootPath, DatFileHelper.DAT_BANNER_FOLDER);
                    datExisting.Image = datNewImage;
                }

                datExisting.Name = datBanner.Name;
                datExisting.Description = datBanner.Description;
                datExisting.Status = datBanner.Status;
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Cập nhật banner thành công";
                return RedirectToAction(nameof(DatIndex));
            }
            return View(datBanner);
        }

        public async Task<IActionResult> DatDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datBanner = await _datContext.DatBanners.FirstOrDefaultAsync(datB => datB.Id == id);
            if (datBanner == null)
            {
                return NotFound();
            }
            return View(datBanner);
        }

        [HttpPost, ActionName("DatDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatDeleteConfirmed(int id)
        {
            var datBanner = await _datContext.DatBanners.FindAsync(id);
            if (datBanner != null)
            {
                DatFileHelper.DatDeleteImage(datBanner.Image, _datEnvironment.WebRootPath, DatFileHelper.DAT_BANNER_FOLDER);
                _datContext.DatBanners.Remove(datBanner);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Xóa banner thành công";
            }
            return RedirectToAction(nameof(DatIndex));
        }
    }
}
