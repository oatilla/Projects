using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class ProductPage
    {
        public tblCategory Category { get; set; }
        public tblProductDetail ProductDetail { get; set; }
        public IEnumerable<tblCategory> CategoryListSameCategory { get; set; }
        public IEnumerable<tblCategory> CategoryListParentCategory { get; set; }
        public IEnumerable<tblCategory> CategoryListParentChildCategory { get; set; }
    }
}