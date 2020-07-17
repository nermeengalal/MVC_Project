using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace Day7.Models
{
    public class CurrentOrderDetails
    {
        public int OrderId { get; set; }

        public DateTime Date { get; set; }

        public PaymentMethod Payment { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }


        public string PhoneNumber { get; set; }

        public string ProductName { get; set; }

        public int ProductQuantity { get; set; }

        public int Price { get; set; }

        public int TotalPrice { get; set; }
    }
}