using Day7.Models;
using Microsoft.Ajax.Utilities;
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
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        //Roles.GetRolesForUser().Contains("Administrator")
        ApplicationDBContext context = new ApplicationDBContext();
        // GET: Manager
        public ActionResult Index()
        {

            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> manager =
                new UserManager<ApplicationUser>(store);

            List<ApplicationUser> users = context.Users.ToList();
            Dictionary<string, string> UserRolesList =
             new Dictionary<string, string>();
            List<string> UsersID = new List<string>();
            foreach (var user in users)
            {
                string role = manager.GetRoles(user.Id).FirstOrDefault();
                if (role != null)
                {
                    if (role.CompareTo("Admin") == 0)
                    {
                        UserRolesList.Add(user.UserName, "Admin");
                        UsersID.Add(user.Id);
                    }
                    else if (role.CompareTo("Manager") == 0)
                    {
                        UserRolesList.Add(user.UserName, "Manager");
                        UsersID.Add(user.Id);
                    }
                }
            }

            ViewBag.Ids = UsersID;
            return View(UserRolesList);
        }

        public ActionResult Employees(string Emp)
        {
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> manager =
                new UserManager<ApplicationUser>(store);

            List<ApplicationUser> emps = context.Users.Where(e => e.UserName.Contains(Emp)).ToList();

            Dictionary<string, string> UserRolesList =
             new Dictionary<string, string>();

            List<string> UsersID = new List<string>();

            foreach (var user in emps)
            {
                string role = manager.GetRoles(user.Id).FirstOrDefault();
                if (role != null)
                {
                    if (role.CompareTo("Admin") == 0)
                    {
                        UserRolesList.Add(user.UserName, "Admin");
                        UsersID.Add(user.Id);
                    }
                    else if (role.CompareTo("Manager") == 0)
                    {
                        UserRolesList.Add(user.UserName, "Manager");
                        UsersID.Add(user.Id);
                    }
                }
            }

            ViewBag.Ids = UsersID;
            ViewBag.EmpName = Emp;
            return View("Index", UserRolesList);
        }
        public ActionResult Registration()
        {
            ViewBag.Roles = context.Roles.Where(r => r.Name != "User").ToList();
            //Applicatiouser "Adress
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Registration(ManagerRegistration userRegister)
        {

            IdentityRole role = new IdentityRole();
            ViewBag.Roles = context.Roles.Where(r => r.Name != "User").ToList();
            if (!ModelState.IsValid)
                return View(userRegister);
            try
            {
                //Creat UserManager
               
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
                IdentityResult resulte = await manager.CreateAsync(user, userRegister.Password);
                if (resulte.Succeeded)
                {
                   
                    
                    manager.AddToRole(user.Id,userRegister.Role);
                   
                    //MAnager SignIn
                    IAuthenticationManager authenticationManager =
                        HttpContext.GetOwinContext().Authentication;
                    SignInManager<ApplicationUser, string> signinmanager =
                        new SignInManager<ApplicationUser, string>
                        (manager, authenticationManager);
                   // signinmanager.SignIn(user, true, true);
                    //Cookie Authorize
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", (resulte.Errors.ToList())[0]);
                    return View(userRegister);

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(userRegister);
            }
        }


      
        public ActionResult Delete(string id)
        {
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> manager =
                new UserManager<ApplicationUser>(store);
            ApplicationUser user = context.Users.Where(u => u.Id == id).FirstOrDefault();
            manager.RemoveFromRole(id, "Admin");
            manager.Delete(user);
            return RedirectToAction("index");
        }
    }
}