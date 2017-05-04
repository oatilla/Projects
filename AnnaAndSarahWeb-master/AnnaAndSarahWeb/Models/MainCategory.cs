using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class MainCategory
    {
        public string CategoryName { get; set; }
        public int CatId { get; set; }
        public bool IsEnabled { get; set; }
        public HttpPostedFileBase ProductImage { get; set; }
    }
}