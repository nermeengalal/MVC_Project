using Day7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Day7.Controllers
{
    //user
    public class AccountController : Controller
    {
        ApplicationDBContext context = new ApplicationDBContext();
        // GET: Account
        public ActionResult Registration()
        {
            //Applicatiouser "Adress
            return View();
        }
        [HttpPost]
        public async  Task<ActionResult> Registration(RegistrationViewModel userRegister)
        {
            if(!ModelState.IsValid)
                return View(userRegister);
            try
            {
                //Creat UserManager
                ApplicationDBContext context = new ApplicationDBContext();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> manager = 
                    new UserManager<ApplicationUser>(store);
                //Map Vm To Model
                ApplicationUser user = new ApplicationUser();
                user.UserName = userRegister.Username;
                user.PasswordHash = userRegister.Password;
                user.Email = userRegister.Email;
                user.Address = userRegister.Adress;

                //UserManager save user database
                IdentityResult resulte =await manager.CreateAsync(user, userRegister.Password);
                if (resulte.Succeeded)
                {
                    manager.AddToRole(user.Id, "User");
                
                    //MAnager SignIn
                    IAuthenticationManager authenticationManager =
                        HttpContext.GetOwinContext().Authentication;
                    SignInManager<ApplicationUser, string> signinmanager =
                        new SignInManager<ApplicationUser, string>
                        (manager, authenticationManager);
                    signinmanager.SignIn(user, true, true);
                    //Cookie Authorize
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("",(resulte.Errors.ToList())[0]);
                    return View(userRegister);

                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(userRegister);
            }
        }

        public ActionResult Delete(ApplicationUser User)
        {
            ApplicationDBContext context = new ApplicationDBContext();
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> manager =
                new UserManager<ApplicationUser>(store);

            manager.RemoveFromRole<ApplicationUser, string>(User.UserName, "Admin");

            return View("AdminList");
        }

        //public ActionResult AdminList()
        //{
        //    IdentityRole role = new IdentityRole();
        //    IdentityUserRole role1 = new IdentityUserRole();
        //    List<ApplicationUser> users = context.Users.Where(u => u.Roles.Where(role1.RoleId == role.Id) && role.Name == "Admin");
        //}

        //Roles.GetRolesForUser().Contains("Administrator")
    }
}