using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnaAndSarahWeb.Models
{
    public class AnaSayfaPage
    {
        public string MailAdress { get; set; }
        public string Search { get; set; }
        public IEnumerable<tblTag> TagList { get; set; }
        public List<tblCategory> CategoryNuts { get; set; }
        public List<tblCategory> CategoryDriedFruits { get; set; }
        public List<tblCategory> CategorySeeds { get; set; }
        public List<tblCategory> CategorySnacks { get; set; }
        public List<tblCategory> CategoryGifts { get; set; }
        public List<tblBanner> Banners { get; set; }
    }
}