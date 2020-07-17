using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Day7.Models
{
    public class Card
    {
        public int ID { get; set; }
     
        public virtual ICollection<Product> Products { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        public int Quantity { get; set; }


        [Display(Name ="Price")]
        public int PriceOfEachProduct { get; set; }
    }
}