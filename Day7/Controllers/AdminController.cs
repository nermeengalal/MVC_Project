using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day7.Models;
using System.IO;
using PagedList;
using Microsoft.AspNet.Identity;

namespace Day7.Controllers
{
    [Authorize(Roles = "Admin,Manager")]

    //[Authorize(Roles = "Manager")]

    public class AdminController : Controller
    {
        ApplicationDBContext context = new ApplicationDBContext();
        

        public ActionResult ShowMenu()
        {
            List<Category> cat = context.categories.ToList();
            return PartialView("Menu",cat);
        }


        // GET: Admin
        public ActionResult Index(int? page)
        {
            List<Product> pro = context.products.ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(pro.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Getbycat(int id, int? page)
        {
            List<Product> pro = context.products.Where(p => p.CatID == id).ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("Index", pro.ToPagedList(pageNumber, pageSize));
        }


        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.Category = context.categories.ToList();
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(Product pro, HttpPostedFileBase file)
        {
            ViewBag.Category = context.categories.ToList();
            try
            {
                string ImageName = Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/Content/Products-Pictures/" + ImageName);
                file.SaveAs(physicalPath);
                pro.Picture = ImageName;
                pro.UploadDate = DateTime.Now;
                pro.UserID=System.Web.HttpContext.Current.User.Identity.GetUserId();
                context.products.Add(pro);

                context.SaveChanges();
              
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(pro);
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Category = context.categories.ToList();
            Product s = context.products.FirstOrDefault(p => p.ID == id);
            return View(s);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Product pro, HttpPostedFileBase file)
        {
            //string path = Path.Combine(Server.MapPath("~/Content/Products-Pictures"), upload.FileName);

            // upload.SaveAs(path);

            //file.SaveAs(HttpContext.Server.MapPath("~/Content/Products-Pictures") + file.FileName);
            //customer.picture = file.FileName;
            string ImageName = Path.GetFileName(file.FileName);
            string physicalPath = Server.MapPath("~/Content/Products-Pictures/" + ImageName);
            file.SaveAs(physicalPath);

            ViewBag.Category = context.categories.ToList();
            
            try
            {
                Product Oldpro = context.products.FirstOrDefault(p => p.ID == id);
                Oldpro.Name = pro.Name;
                Oldpro.Description = pro.Description;
                Oldpro.UploadDate = DateTime.Now;
                Oldpro.UserID = System.Web.HttpContext.Current.User.Identity.GetUserId(); 
                Oldpro.Picture = ImageName;
                Oldpro.Price = pro.Price;
                Oldpro.RemainedQuantity = pro.RemainedQuantity;
                Oldpro.TotalQuantity = pro.TotalQuantity;
                Oldpro.CompanyName = pro.CompanyName;
                Oldpro.CatID = pro.CatID;

                context.SaveChanges();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(pro);
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            Product pro = context.products.FirstOrDefault(p => p.ID == id);
            context.products.Remove(pro);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddCat()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCat(Category cat)
        {
            try
            {
                context.categories.Add(cat);

                context.SaveChanges();

                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(cat);
            }
        }

        public ActionResult DeleteCat(int id)
        {
            Category cat = context.categories.FirstOrDefault(c => c.ID == id);
            context.categories.Remove(cat);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SearchForProduct(string Pname,int? page)
        {
            ViewBag.Pname = Pname;
            List<Product> pro = context.products.Where(p => p.Name.Contains(Pname)).ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("Index", pro.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CheckRemainQuantity(int RemainedQuantity, int TotalQuantity)
        {
            Product p = context.products.FirstOrDefault(pro => pro.RemainedQuantity == RemainedQuantity && pro.TotalQuantity == TotalQuantity);
            if (RemainedQuantity <= TotalQuantity)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

    }
}
