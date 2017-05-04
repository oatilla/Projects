using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class PBlog
    {
        public HttpPostedFileBase file { get; set; }
        public tblBlog Blog { get; set; }
        public int Cid { get; set; }
    }
}