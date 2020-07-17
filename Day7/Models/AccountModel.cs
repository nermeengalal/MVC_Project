using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Day7.Models
{
    //Business Entity
  
    //IdentityUser:Iuse
    public class ApplicationUser : IdentityUser
    {

        public string Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext():base("OnlineStore")
        {

        }
   
        public ApplicationDBContext(string connectioname):base(connectioname)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Card> cards { get; set; }

    }
}