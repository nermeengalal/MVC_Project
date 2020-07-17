using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Day7.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime UploadDate { get; set; }

        public int ID { get; set; }


        public int RemainedQuantity { get; set; }

        public int Price { get; set; }

        public string Picture { get; set; }

        public string CompanyName { get; set; }

        public int QuantityNeeded { get; set; }
    }
}