using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Day7.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        ApplicationDBContext context = new ApplicationDBContext();
       
        // GET: Profile
      

        public ActionResult GetUser()
        {
            string name = System.Web.HttpContext.Current.User.Identity.GetUserName();
            //name = ViewBag.name;
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> manager =
                new UserManager<ApplicationUser>(store);

            ApplicationUser user = context.Users.FirstOrDefault(u => u.UserName == name);
            
            //var user = System.Web.HttpContext.Current.User.Identity.GetUserName();
            return View(user);
        }

        public ActionResult GetUserForEditing()
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //old user
            ApplicationUser user = context.Users.Where(us => us.Id == id).FirstOrDefault();
            return View(user);
        }

        public ActionResult EditUser(string id, ApplicationUser u)
        {
            //old user
            ApplicationUser user = context.Users.Where(us => us.Id == id).FirstOrDefault();
            user.Address = u.Address;
            user.UserName = u.UserName;
            user.Email = u.Email;
            user.PhoneNumber = u.PhoneNumber;
            user.EmailConfirmed = u.EmailConfirmed;
            user.PhoneNumberConfirmed = u.PhoneNumberConfirmed;
            context.SaveChanges();
            return View("GetUser",u);
        }



        public ActionResult ChangePass()
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //old user
            ApplicationUser user = context.Users.Where(us => us.Id == id).FirstOrDefault();
            RegistrationViewModel u = new RegistrationViewModel();
            user.UserName = u.Username;
            user.PasswordHash = u.Password;
            user.Email = u.Email;
            user.Address = u.Adress;
            ViewBag.pass = user.PasswordHash;
            return View(u);
        }
        public ActionResult EditPass( RegistrationViewModel u )
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = context.Users.Where(us => us.Id == id).FirstOrDefault();
        
            user.PasswordHash = u.Password;
;
            context.SaveChanges();
            return View("GetUser", user);
           
        }
    }

    
}