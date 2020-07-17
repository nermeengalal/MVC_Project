using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Day7.Startup1))]

namespace Day7
{
    //Owin
    public class Startup1
    {
        //Application_Start
        public void Configuration(IAppBuilder app)
        {
            //Use Cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType=DefaultAuthenticationTypes.ApplicationCookie,
                //NAme
                ExpireTimeSpan=TimeSpan.FromDays(1),
                //Life Tim
                //LoginPath=new PathString(@"/Account/Login"),
                //Redirect Login
            });
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
