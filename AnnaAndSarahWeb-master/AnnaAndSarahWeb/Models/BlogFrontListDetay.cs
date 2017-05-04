using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class BlogFrontListDetay
    {
        public vw_blog_category BlogCategory { get; set; }
        public int CommentCount
        {
            get; set;
        }
        public tblBlog Blog { get; set; }
        public List<tblComment> CommentList { get; set; }
        public List<tblBlog> PopulerBlogList { get; set; }
        public List<BlogFrontListDetay> BlogCategoryList { get; set; }

    }
}