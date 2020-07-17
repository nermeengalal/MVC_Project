using Day7.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Day7.Controllers
{
    public class UserController : Controller
    {
        ApplicationDBContext context = new ApplicationDBContext();

        // GET: Admin
        public ActionResult Index(int? page)
        {
            List<Product> pro = context.products.ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(pro.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ShowMenu()
        {
            List<Category> cat = context.categories.ToList();
            return PartialView("Menu", cat);
        }

        public ActionResult Getbycat(int id ,int? page)
        {
            List<Product> pro = context.products.Where(p => p.CatID == id).ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("Index",pro.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SearchForProduct(string Pname, int? page)
        {
            ViewBag.Pname = Pname;
            List<Product> pro = context.products.Where(p => p.Name.Contains(Pname)).ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View("Index", pro.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id)
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

        public ActionResult card(int id, ProductViewModel p)
        {
            Product pro = context.products.FirstOrDefault(pr => pr.ID == id);
            p.Name = pro.Name;
            p.Price = pro.Price*p.QuantityNeeded ;
            List<ProductViewModel> prod = Session["Count"] as List<ProductViewModel>;
            Session["ProductViewModel"] =new List<ProductViewModel>();
            int Tprice = 0;
            if (p != null)
            {
                if (prod != null)
                {
                    foreach (var item in prod)
                    {
                        ((List<ProductViewModel>)Session["ProductViewModel"]).Add(item);
                        Tprice+= p.Price;
                    }
                }
                 ((List<ProductViewModel>)Session["ProductViewModel"]).Add(p);
            }

            return RedirectToAction("GetAllItemsInCard");
        }

        public ActionResult GetAllItemsInCard()
        {
            List<ProductViewModel> pro = new List<ProductViewModel>();
            
            if (Session["ProductViewModel"] != null)
            {   
                //ProductViewModel p = Session["ProductViewModel" + Session["Count"]] as ProductViewModel; 
                pro=Session["ProductViewModel"] as List<ProductViewModel>;
                ViewBag.product= Session["ProductViewModel"] as List<ProductViewModel>;
                //Session["ProductViewModel"] = null; 

                return View(pro);
            }
            else
            {
                return View();
            }   
        }

        public ActionResult DeleteFromCard(int id)
        {
            List<ProductViewModel> pro = new List<ProductViewModel>();

            if (Session["ProductViewModel"] != null)
            {
                
                pro = Session["ProductViewModel"] as List<ProductViewModel>;
                pro.RemoveAt(id);
                
                ViewBag.product = pro;
            }
            return RedirectToAction("GetAllItemsInCard",pro);
        }

        public ActionResult BeforeConfirmation()
        {
            Order o = new Order();
            int totalprice = 0;
            foreach (var item in Session["ProductViewModel"] as List<ProductViewModel>)
            {
                totalprice += item.Price;
            }
            ViewBag.totalprice = totalprice;
            return View(o);
        }

        public ActionResult ConfirmOrder(Order o)
        {
            Order order = new Order();
            order.Date = DateTime.Now;
            order.UserID = System.Web.HttpContext.Current.User.Identity.GetUserId();
            foreach (var item in Session["ProductViewModel"] as List<ProductViewModel>)
            {
                order.TotalPrice += item.Price;
               
            }
            order.Payment = o.Payment;
            context.Orders.Add(order);
            context.SaveChanges();
            return RedirectToAction("SaveInCard");
        }

        public ActionResult SaveInCard()
        {

            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            Order O = context.Orders.OrderByDescending(or => or.Id).FirstOrDefault(or => or.UserID == id);


            Card card = new Card();
            card.OrderID = O.Id;
            int allprice =0;
            int allQuantity = 0;
            foreach (var item in Session["ProductViewModel"] as List<ProductViewModel>)
            {
                allprice += item.Price;
                allQuantity += item.QuantityNeeded;
            }
            card.PriceOfEachProduct = allprice;
            card.Quantity = allQuantity;
            List <ProductViewModel> pr = Session["ProductViewModel"] as List<ProductViewModel>;
            List<Product> pro = new List<Product>();
            foreach (var item in pr)
            {
                pro.Add(context.products.Where(p => p.Name.Contains(item.Name)).FirstOrDefault());
            }
            card.Products = pro;
            context.cards.Add(card);
            context.SaveChanges();
            return RedirectToAction("CalculateRemainedQuantity");
        }

        public ActionResult CalculateRemainedQuantity()
        {
            List<ProductViewModel> pr = Session["ProductViewModel"] as List<ProductViewModel>;
            Product pro = new Product();
            List<CurrentOrderDetails> current = new List<CurrentOrderDetails>();
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            Order O = context.Orders.OrderByDescending(or => or.Id).FirstOrDefault(or => or.UserID == id);
            ApplicationUser user = context.Users.Where(u => u.Id == id).FirstOrDefault();

            foreach (var item in pr)
            {
                pro=context.products.Where(pp=> pp.Name.Contains(item.Name)).FirstOrDefault();
                pro.RemainedQuantity -= item.QuantityNeeded;

                //if(current.Price!=null && current.ProductName !=null && current.ProductQuantity != null)
                //{
                current.Add(new CurrentOrderDetails { ProductName = item.Name,Price=item.Price,ProductQuantity=item.QuantityNeeded });
                    //current.ProductQuantity.Add(item.QuantityNeeded);
                    //current.Price.Add(item.Price);
                ///}
               

                context.SaveChanges();
            }
            current[0].UserName = user.UserName;
            current[0].Address = user.Address;
            current[0].PhoneNumber = user.PhoneNumber;
            current[0].OrderId = O.Id;
            current[0].Date = O.Date;
            current[0].Payment = O.Payment;
            current[0].TotalPrice = O.TotalPrice;

            Session["ProductViewModel"] = null;
            Session["Count"] = null;
            return View(current);
        }

        public ActionResult ShowMyOrders()
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            List<Order> order = context.Orders.Where(o => o.UserID == id).ToList();
            List<Card> cards=new List<Card>();
            List<DateTime> d = new List<DateTime>();
            foreach (var item in order)
            {
                d.Add( item.Date);
                cards.Add(context.cards.Where(c => c.OrderID == item.Id).FirstOrDefault());
            }
            ViewBag.date = d;
            return View(cards);
        }

        public ActionResult OrderDetail(int id)
        {
            Card card = context.cards.Where(c => c.ID == id).FirstOrDefault();
            List<Product> pro = new List<Product>();
            foreach (var item in card.Products)
            {
                pro.Add(item);
            }

            return View(pro);
        }
    }
}
