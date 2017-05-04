using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class BannerPage
    {
        public tblBanner banner { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}