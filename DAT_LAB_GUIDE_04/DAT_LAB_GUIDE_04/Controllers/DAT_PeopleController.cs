using DAT_LAB_GUIDE_04.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace DAT_LAB_GUIDE_04.Controllers
{
    public class DAT_PeopleController : Controller
    {
        // GET: DAT_People
        public ActionResult Index()
        {
            var _peoples = DAT_DataLocal.GetPeoples();
            return View(_peoples);
        }

        // GET: DAT_People/Details/5
        public ActionResult Details(int id)
        {
            var people = DAT_DataLocal.GetPeopleById(id);
            return View(people);
        }

        // GET: DAT_People/Create
        public ActionResult Create()
        {
            var people = new DAT_People();
            return View(people);
        }

        // POST: DAT_People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DAT_People model)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var fileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\avatar", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    model.Avatar = "images/avatar/" + fileName;
                }

                DAT_DataLocal.peoples.Add(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View(model);
            }
        }

        // GET: DAT_People/Edit/5
        public ActionResult Edit(int id)
        {
            var people = DAT_DataLocal.GetPeopleById(id);
            return View(people);
        }

        // POST: DAT_People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DAT_People model)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var fileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\avatar", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    model.Avatar = "images/avatar/" + fileName;
                }

                for (int i = 0; i < DAT_DataLocal.peoples.Count; i++)
                {
                    if (DAT_DataLocal.peoples[i].Id == id)
                    {
                        DAT_DataLocal.peoples[i] = model;
                        break;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: DAT_People/Delete/5
        public ActionResult Delete(int id)
        {
            var people = DAT_DataLocal.GetPeopleById(id);
            return View(people);
        }

        // POST: DAT_People/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DAT_People model)
        {
            try
            {
                for (int i = 0; i < DAT_DataLocal.peoples.Count; i++)
                {
                    if (DAT_DataLocal.peoples[i].Id == id)
                    {
                        DAT_DataLocal.peoples.RemoveAt(i);
                        break;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}