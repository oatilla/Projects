using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class BlogFrontList
    {
        public List<BlogPaket> BlogPaketList { get; set; }        
        public List<tblBlogCategory> CategoryWidgetList { get; set; }
        public List<tblBlog> BlogWidgetList { get; set; }
    }
}