using Day7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Day7.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel userLogin)
        {
            if (!ModelState.IsValid)
                return View(userLogin);
            try
            {
                ApplicationDBContext context = new ApplicationDBContext();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);

                ApplicationUser user = new ApplicationUser();
                user.UserName = userLogin.Username;
                user.PasswordHash = userLogin.Password;

                var AppUser = manager.Find(user.UserName, user.PasswordHash);

                if (AppUser != null)
                {
                    IAuthenticationManager authenticationManager =
                      HttpContext.GetOwinContext().Authentication;
                    SignInManager<ApplicationUser, string> signinmanager =
                        new SignInManager<ApplicationUser, string>
                        (manager, authenticationManager);
                    signinmanager.SignIn(AppUser, true, true);
                    //Cookie Authorize
                    string role = manager.GetRoles(AppUser.Id).FirstOrDefault();
                    if(role.CompareTo("Admin") == 0)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if(role.CompareTo("Manager") == 0)
                    {
                        return RedirectToAction("Index", "Manager");
                    }
                    else if (role.CompareTo("User") == 0)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password isnt Correct");
                    return View(userLogin);

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(userLogin);
            }
        }


        [Authorize]
        public ActionResult Signout()
        {
            IAuthenticationManager manger = HttpContext.GetOwinContext().Authentication;

            manger.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}