using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAT_NetCoreMVC_LAB5.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAT_NetCoreMVC_LAB5.Controllers
{
    public class DAT_ProductController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private static List<DAT_Category> categories = new List<DAT_Category>
        {
            new DAT_Category { Id = 1, Name = "Điện thoại" },
            new DAT_Category { Id = 2, Name = "Laptop" },
            new DAT_Category { Id = 3, Name = "Phụ kiện" }
        };

        private static List<DAT_Product> products = new List<DAT_Product>
{
    new DAT_Product
    {
        Id = 1,
        Name = "iPhone 18 Pro Max 2TB",
        Price = 95000000,
        SalePrice = 88500000,
        CategoryId = 1,
        Image = "wwwroot/products/1.jpg",
        Description = "Điện thoại flagship cao cấp mới nhất"
    }
};
        public DAT_ProductController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // GET: DAT_Product/Index
        public IActionResult Index()
        {
            ViewBag.Categories = categories;
            return View(products);
        }

        // GET: DAT_Product/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: DAT_Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DAT_Product model)
        {
            if (ModelState.IsValid)
            {
                // Xử lý lưu ảnh vào thư mục wwwroot/products
                if (model.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(fileStream);
                    }

                    model.Image = "/products/" + uniqueFileName;
                }

                model.Id = products.Count + 1;
                products.Add(model);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", model.CategoryId);
            return View(model);
        }
    }
}