using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Day7.Models;
using System.Threading.Tasks;

namespace Day7.Controllers
{
    public class RoleController : Controller
    {
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> New(string RoleName)
        {
            if (RoleName != null)
            {
                ApplicationDBContext context = new ApplicationDBContext();
                RoleStore<IdentityRole> store = new RoleStore<IdentityRole>(context);
                RoleManager<IdentityRole> manager = new RoleManager<IdentityRole>(store);

                IdentityRole role = new IdentityRole();
                role.Name = RoleName;
                IdentityResult result = await manager.CreateAsync(role);
                if (result.Succeeded)
                    return View();//Admin
                else
                {
                    ViewBag.Error = result.Errors;
                    ViewBag.RoleName = RoleName;
                    return View();
                }
            }
            return View();
        }
    }
}