using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Galgale.Models;

namespace Galgale.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage

        DB090928093827Entities entityGalgale = new DB090928093827Entities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SiteEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SiteEkle(tbl_PortalHaber_Site ss)
        {
            entityGalgale.tbl_PortalHaber_Site.Add(ss);
            int dd = entityGalgale.SaveChanges();
            if (dd > 0)
            {
                return RedirectToAction("SiteEkle");
            }
            else
            {
                return RedirectToAction("SiteEkle");
            }

            
        }

        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KategoriEkle(tbl_PortalHaber_kategori ss)
        {
            entityGalgale.tbl_PortalHaber_kategori.Add(ss) ;
            int dd = entityGalgale.SaveChanges();
            if (dd > 0)
            {
                return RedirectToAction("KategoriEkle");
            }
            else
            {
                return RedirectToAction("KategoriEkle");
            }


        }

        public ActionResult TipEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TipEkle(tbl_PortalHaber_Tip ss)
        {
            entityGalgale.tbl_PortalHaber_Tip.Add(ss);
            int dd = entityGalgale.SaveChanges();
            if (dd > 0)
            {
                return RedirectToAction("TipEkle");
            }
            else
            {
                return RedirectToAction("SiteEkle");
            }


        }

        public ActionResult YazarEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YazarEkle(tbl_PortalHaber_Yazar ss)
        {
            entityGalgale.tbl_PortalHaber_Yazar.Add(ss);
            int dd = entityGalgale.SaveChanges();
            if (dd > 0)
            {
                return RedirectToAction("YazarEkle");
            }
            else
            {
                return RedirectToAction("YazarEkle");
            }


        }
        public ActionResult KaynakEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KaynakEkle(tbl_PortalHaber_Kaynak ss)
        {
            entityGalgale.tbl_PortalHaber_Kaynak.Add(ss);
            int dd = entityGalgale.SaveChanges();
            if (dd > 0)
            {
                return RedirectToAction("KaynakEkle");
            }
            else
            {
                return RedirectToAction("KaynakEkle");
            }


        }
        public ActionResult IcerikEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IcerikEkle(tbl_PortalHaber_Icerik ss)
        {
            entityGalgale.tbl_PortalHaber_Icerik.Add(ss);
            int dd = entityGalgale.SaveChanges();
            if (dd > 0)
            {
                return RedirectToAction("IcerikEkle");
            }
            else
            {
                return RedirectToAction("IcerikEkle");
            }


        }



    }
}
