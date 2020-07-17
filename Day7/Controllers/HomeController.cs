using Day7.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Day7.Controllers
{
    
    public class HomeController : Controller
    {
        ApplicationDBContext context = new ApplicationDBContext();
        public ActionResult ShowMenu()
        {
            List<Category> cat = context.categories.ToList();
            return PartialView("_CatView", cat);
        }


        // GET: Admin
        public ActionResult Index(int? page)
        {
            List<Product> pro = context.products.ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(pro.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Getbycat(int id,int? page)
        {
            List<Product> pro = context.products.Where(p => p.CatID == id).ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("Index", pro.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id )
        {
            Product pro = context.products.FirstOrDefault(p => p.ID == id);
            ProductViewModel pr = new ProductViewModel();
            pr.CompanyName = pro.CompanyName;
            pr.Description = pro.Description;
            pr.ID = pro.ID;
            pr.Name = pro.Name;
            pr.Picture = pro.Picture;
            pr.RemainedQuantity = pro.RemainedQuantity;
            pr.Price = pro.Price;
            pr.UploadDate = pro.UploadDate;
            return View(pr);
        }
        public ActionResult SearchForProduct(string Pname,int? page)
        {
            ViewBag.Pname = Pname;
            List<Product> pro = context.products.Where(p => p.Name.Contains(Pname)).ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("Index", pro.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult card(int id ,ProductViewModel p)
        {
            return View(p);
        }
    }
}