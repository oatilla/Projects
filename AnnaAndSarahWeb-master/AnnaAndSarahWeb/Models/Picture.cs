using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class Picture
    {
        public tblCategory getCategory { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}