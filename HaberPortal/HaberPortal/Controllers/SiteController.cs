using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HaberPortal.Models;

namespace HaberPortal.Controllers
{
    public class SiteController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: Site
        public ActionResult Index()
        {
            var tbl_PortalHaber_Site = db.tbl_PortalHaber_Site.Include(t => t.tbl_PortalHaber_SiteGrubu);
            return View(tbl_PortalHaber_Site.ToList());
        }

        // GET: Site/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Site tbl_PortalHaber_Site = db.tbl_PortalHaber_Site.Find(id);
            if (tbl_PortalHaber_Site == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Site);
        }

        // GET: Site/Create
        public ActionResult Create()
        {
            ViewBag.Site_Grubu = new SelectList(db.tbl_PortalHaber_SiteGrubu, "Site_Grup_Id", "Site_Grup_Aciklama");
            return View();
        }

        // POST: Site/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Site_Id,Site_Adi,Site_Grubu")] tbl_PortalHaber_Site tbl_PortalHaber_Site)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_Site.Add(tbl_PortalHaber_Site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Site_Grubu = new SelectList(db.tbl_PortalHaber_SiteGrubu, "Site_Grup_Id", "Site_Grup_Aciklama", tbl_PortalHaber_Site.Site_Grubu);
            return View(tbl_PortalHaber_Site);
        }

        // GET: Site/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Site tbl_PortalHaber_Site = db.tbl_PortalHaber_Site.Find(id);
            if (tbl_PortalHaber_Site == null)
            {
                return HttpNotFound();
            }
            ViewBag.Site_Grubu = new SelectList(db.tbl_PortalHaber_SiteGrubu, "Site_Grup_Id", "Site_Grup_Aciklama", tbl_PortalHaber_Site.Site_Grubu);
            return View(tbl_PortalHaber_Site);
        }

        // POST: Site/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Site_Id,Site_Adi,Site_Grubu")] tbl_PortalHaber_Site tbl_PortalHaber_Site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_Site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Site_Grubu = new SelectList(db.tbl_PortalHaber_SiteGrubu, "Site_Grup_Id", "Site_Grup_Aciklama", tbl_PortalHaber_Site.Site_Grubu);
            return View(tbl_PortalHaber_Site);
        }

        // GET: Site/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Site tbl_PortalHaber_Site = db.tbl_PortalHaber_Site.Find(id);
            if (tbl_PortalHaber_Site == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Site);
        }

        // POST: Site/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_Site tbl_PortalHaber_Site = db.tbl_PortalHaber_Site.Find(id);
            db.tbl_PortalHaber_Site.Remove(tbl_PortalHaber_Site);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
