using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Day7.Models
{
    public class Product
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime UploadDate { get; set; }

        public int ID { get; set; }
        [Required]
        public int TotalQuantity { get; set; }
        [Remote(action: "CheckRemainQuantity", controller: "Admin", ErrorMessage = "Remained Quantity must be less than or equal Total Degree", AdditionalFields = "TotalQuantity")]
        [Required]
        public int RemainedQuantity { get; set; }
        [Required]
        public int Price { get; set; }

        public string Picture { get; set; }

        public string  CompanyName { get; set; }
       [Required]
        public int CatID { get; set; }
        [ForeignKey("CatID")]

        public virtual Category Category { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser AppUser { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}