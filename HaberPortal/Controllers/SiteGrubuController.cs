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
    public class SiteGrubuController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: SiteGrubu
        public ActionResult Index()
        {
            return View(db.tbl_PortalHaber_SiteGrubu.ToList());
        }

        // GET: SiteGrubu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_SiteGrubu tbl_PortalHaber_SiteGrubu = db.tbl_PortalHaber_SiteGrubu.Find(id);
            if (tbl_PortalHaber_SiteGrubu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_SiteGrubu);
        }

        // GET: SiteGrubu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SiteGrubu/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Site_Grup_Id,Site_Grup_Aciklama")] tbl_PortalHaber_SiteGrubu tbl_PortalHaber_SiteGrubu)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_SiteGrubu.Add(tbl_PortalHaber_SiteGrubu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_PortalHaber_SiteGrubu);
        }

        // GET: SiteGrubu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_SiteGrubu tbl_PortalHaber_SiteGrubu = db.tbl_PortalHaber_SiteGrubu.Find(id);
            if (tbl_PortalHaber_SiteGrubu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_SiteGrubu);
        }

        // POST: SiteGrubu/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Site_Grup_Id,Site_Grup_Aciklama")] tbl_PortalHaber_SiteGrubu tbl_PortalHaber_SiteGrubu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_SiteGrubu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_PortalHaber_SiteGrubu);
        }

        // GET: SiteGrubu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_SiteGrubu tbl_PortalHaber_SiteGrubu = db.tbl_PortalHaber_SiteGrubu.Find(id);
            if (tbl_PortalHaber_SiteGrubu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_SiteGrubu);
        }

        // POST: SiteGrubu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_SiteGrubu tbl_PortalHaber_SiteGrubu = db.tbl_PortalHaber_SiteGrubu.Find(id);
            db.tbl_PortalHaber_SiteGrubu.Remove(tbl_PortalHaber_SiteGrubu);
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
