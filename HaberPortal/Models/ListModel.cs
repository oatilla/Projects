using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberPortal.Models
{
    public class ListModel
    {
        public List<ListTable> KategoriList { get; set; }
        public List<ListTable> KaynakList { get; set; }
        public List<ListTable> SiteList { get; set; }
        public List<ListTable> BannerList { get; set; }
    }
}