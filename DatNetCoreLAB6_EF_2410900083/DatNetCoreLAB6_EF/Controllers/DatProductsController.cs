using DatNetCoreLAB6_EF.Data;
using DatNetCoreLAB6_EF.Helpers;
using DatNetCoreLAB6_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DatNetCoreLAB6_EF.Controllers
{
    // Bài tự làm 1: CRUD bảng DatProduct có upload ảnh
    public class DatProductsController : Controller
    {
        private readonly DatAppDbContext _datContext;
        private readonly IWebHostEnvironment _datEnvironment;

        public DatProductsController(DatAppDbContext datContext, IWebHostEnvironment datEnvironment)
        {
            _datContext = datContext;
            _datEnvironment = datEnvironment;
        }

        // GET: DatProducts/DatIndex
        public async Task<IActionResult> DatIndex()
        {
            var datProducts = await _datContext.DatProducts
                .Include(datP => datP.Category)
                .OrderByDescending(datP => datP.Id)
                .ToListAsync();
            return View(datProducts);
        }

        // GET: DatProducts/DatDetails/5
        public async Task<IActionResult> DatDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datProduct = await _datContext.DatProducts
                .Include(datP => datP.Category)
                .FirstOrDefaultAsync(datP => datP.Id == id);
            if (datProduct == null)
            {
                return NotFound();
            }

            return View(datProduct);
        }

        // GET: DatProducts/DatCreate
        public async Task<IActionResult> DatCreate()
        {
            await DatLoadCategoriesAsync(null);
            return View(new DatProduct());
        }

        // POST: DatProducts/DatCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatCreate(
            [Bind("Name,Price,SalePrice,Status,Descriptions,CategoryId")] DatProduct datProduct,
            IFormFile? datImageFile)
        {
            DatValidateProduct(datProduct, datImageFile);

            if (ModelState.IsValid)
            {
                // upload file vào thư mục wwwroot/images/products
                if (datImageFile != null && datImageFile.Length > 0)
                {
                    datProduct.Image = await DatFileHelper.DatSaveImageAsync(
                        datImageFile, _datEnvironment.WebRootPath, DatFileHelper.DAT_PRODUCT_FOLDER);
                }

                datProduct.CreatedDate = DateTime.Now;
                _datContext.Add(datProduct);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction(nameof(DatIndex));
            }

            await DatLoadCategoriesAsync(datProduct.CategoryId);
            return View(datProduct);
        }

        // GET: DatProducts/DatEdit/5
        public async Task<IActionResult> DatEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datProduct = await _datContext.DatProducts.FindAsync(id);
            if (datProduct == null)
            {
                return NotFound();
            }

            await DatLoadCategoriesAsync(datProduct.CategoryId);
            return View(datProduct);
        }

        // POST: DatProducts/DatEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatEdit(int id,
            [Bind("Id,Name,Image,Price,SalePrice,Status,Descriptions,CategoryId")] DatProduct datProduct,
            IFormFile? datImageFile)
        {
            if (id != datProduct.Id)
            {
                return NotFound();
            }

            DatValidateProduct(datProduct, datImageFile);

            if (ModelState.IsValid)
            {
                var datExisting = await _datContext.DatProducts.FindAsync(id);
                if (datExisting == null)
                {
                    return NotFound();
                }

                // Có chọn ảnh mới thì thay ảnh, không thì giữ ảnh cũ
                if (datImageFile != null && datImageFile.Length > 0)
                {
                    var datNewImage = await DatFileHelper.DatSaveImageAsync(
                        datImageFile, _datEnvironment.WebRootPath, DatFileHelper.DAT_PRODUCT_FOLDER);
                    DatFileHelper.DatDeleteImage(datExisting.Image, _datEnvironment.WebRootPath, DatFileHelper.DAT_PRODUCT_FOLDER);
                    datExisting.Image = datNewImage;
                }

                datExisting.Name = datProduct.Name;
                datExisting.Price = datProduct.Price;
                datExisting.SalePrice = datProduct.SalePrice;
                datExisting.Status = datProduct.Status;
                datExisting.Descriptions = datProduct.Descriptions;
                datExisting.CategoryId = datProduct.CategoryId;

                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction(nameof(DatIndex));
            }

            await DatLoadCategoriesAsync(datProduct.CategoryId);
            return View(datProduct);
        }

        // GET: DatProducts/DatDelete/5
        public async Task<IActionResult> DatDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datProduct = await _datContext.DatProducts
                .Include(datP => datP.Category)
                .FirstOrDefaultAsync(datP => datP.Id == id);
            if (datProduct == null)
            {
                return NotFound();
            }

            return View(datProduct);
        }

        // POST: DatProducts/DatDelete/5
        [HttpPost, ActionName("DatDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatDeleteConfirmed(int id)
        {
            var datProduct = await _datContext.DatProducts.FindAsync(id);
            if (datProduct != null)
            {
                DatFileHelper.DatDeleteImage(datProduct.Image, _datEnvironment.WebRootPath, DatFileHelper.DAT_PRODUCT_FOLDER);
                _datContext.DatProducts.Remove(datProduct);
                await _datContext.SaveChangesAsync();
                TempData["DatSuccess"] = "Xóa sản phẩm thành công";
            }
            return RedirectToAction(nameof(DatIndex));
        }

        private void DatValidateProduct(DatProduct datProduct, IFormFile? datImageFile)
        {
            var datImageError = DatFileHelper.DatValidateImage(datImageFile);
            if (datImageError != null)
            {
                ModelState.AddModelError(nameof(DatProduct.Image), datImageError);
            }

            if (datProduct.SalePrice > datProduct.Price)
            {
                ModelState.AddModelError(nameof(DatProduct.SalePrice), "Giá khuyến mãi không được lớn hơn giá bán");
            }
        }

        private async Task DatLoadCategoriesAsync(int? datSelectedId)
        {
            var datCategories = await _datContext.DatCategories.OrderBy(datC => datC.Name).ToListAsync();
            ViewData["CategoryId"] = new SelectList(datCategories, "Id", "Name", datSelectedId);
        }
    }
}
