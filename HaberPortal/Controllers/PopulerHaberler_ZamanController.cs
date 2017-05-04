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
    public class PopulerHaberler_ZamanController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: PopulerHaberler_Zaman
        public ActionResult Index()
        {
            var tbl_PortalHaber_PopulerHaberler_Zaman = db.tbl_PortalHaber_PopulerHaberler_Zaman.Include(t => t.tbl_PortalHaber_Site);
            return View(tbl_PortalHaber_PopulerHaberler_Zaman.ToList());
        }

        // GET: PopulerHaberler_Zaman/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_PopulerHaberler_Zaman tbl_PortalHaber_PopulerHaberler_Zaman = db.tbl_PortalHaber_PopulerHaberler_Zaman.Find(id);
            if (tbl_PortalHaber_PopulerHaberler_Zaman == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_PopulerHaberler_Zaman);
        }

        // GET: PopulerHaberler_Zaman/Create
        public ActionResult Create()
        {
            ViewBag.SiteId = new SelectList(db.tbl_PortalHaber_Site, "Site_Id", "Site_Adi");
            return View();
        }

        // POST: PopulerHaberler_Zaman/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PHZ_Id,Zaman,SiteId")] tbl_PortalHaber_PopulerHaberler_Zaman tbl_PortalHaber_PopulerHaberler_Zaman)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_PopulerHaberler_Zaman.Add(tbl_PortalHaber_PopulerHaberler_Zaman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SiteId = new SelectList(db.tbl_PortalHaber_Site, "Site_Id", "Site_Adi", tbl_PortalHaber_PopulerHaberler_Zaman.SiteId);
            return View(tbl_PortalHaber_PopulerHaberler_Zaman);
        }

        // GET: PopulerHaberler_Zaman/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_PopulerHaberler_Zaman tbl_PortalHaber_PopulerHaberler_Zaman = db.tbl_PortalHaber_PopulerHaberler_Zaman.Find(id);
            if (tbl_PortalHaber_PopulerHaberler_Zaman == null)
            {
                return HttpNotFound();
            }
            ViewBag.SiteId = new SelectList(db.tbl_PortalHaber_Site, "Site_Id", "Site_Adi", tbl_PortalHaber_PopulerHaberler_Zaman.SiteId);
            return View(tbl_PortalHaber_PopulerHaberler_Zaman);
        }

        // POST: PopulerHaberler_Zaman/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PHZ_Id,Zaman,SiteId")] tbl_PortalHaber_PopulerHaberler_Zaman tbl_PortalHaber_PopulerHaberler_Zaman)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_PopulerHaberler_Zaman).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SiteId = new SelectList(db.tbl_PortalHaber_Site, "Site_Id", "Site_Adi", tbl_PortalHaber_PopulerHaberler_Zaman.SiteId);
            return View(tbl_PortalHaber_PopulerHaberler_Zaman);
        }

        // GET: PopulerHaberler_Zaman/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_PopulerHaberler_Zaman tbl_PortalHaber_PopulerHaberler_Zaman = db.tbl_PortalHaber_PopulerHaberler_Zaman.Find(id);
            if (tbl_PortalHaber_PopulerHaberler_Zaman == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_PopulerHaberler_Zaman);
        }

        // POST: PopulerHaberler_Zaman/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_PopulerHaberler_Zaman tbl_PortalHaber_PopulerHaberler_Zaman = db.tbl_PortalHaber_PopulerHaberler_Zaman.Find(id);
            db.tbl_PortalHaber_PopulerHaberler_Zaman.Remove(tbl_PortalHaber_PopulerHaberler_Zaman);
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
