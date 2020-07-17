using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Day7.Models
{
    public class Order
    {
        public int Id { get; set; }
        //public string Description { get; set; }
        public DateTime Date { get; set; }

        public PaymentMethod Payment { get; set; }
        public int TotalPrice { get; set; }
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser AppUser { get; set; }
    }

    public enum PaymentMethod
    {
        Cash = 0, Credit = 1
    }
}