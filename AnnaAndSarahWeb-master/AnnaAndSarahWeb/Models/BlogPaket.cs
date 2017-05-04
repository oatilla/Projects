using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class BlogPaket
    {
        public vw_blog_category blog_category { get; set; }
        public int count { get; set; }
        public tblComment comment { get; set; }
        public List<tblComment> commentList { get; set;}
        public List<tblBlogCategory> CategoryWidgetList { get; set; }
        public List<tblBlog> BlogWidgetList { get; set; }
    }
}