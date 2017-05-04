using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class ProductDetailPage
    {
        public tblProductDetail t_productDetail { get; set; }
        public t_sira_list t_siraList { get; set; }

    }


    public enum t_sira_list
    {
        Bir = 1,
        Iki = 2,
        Uc = 3,
        Dort = 4,
        Bes = 5
    }
}