using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class CategoryTree
    {
        public string search;
        public IEnumerable<tblCategory> CategoryListMain { get; set; }
        public IEnumerable<tblCategory> CategoryListChild { get; set; }
    }
}