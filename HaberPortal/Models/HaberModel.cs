using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberPortal.Models
{
    public class HaberModel
    {
        public int id { get; set; }
        public string EPosta { get; set; }
        public string HaberBaslik { get; set; }
        public string HaberAciklama { get; set; }
    }
}