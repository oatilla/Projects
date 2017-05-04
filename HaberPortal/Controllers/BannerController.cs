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
    public class BannerController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: Banner
        public ActionResult Index()
        {
            return View(db.tbl_PortalHaber_Banner.ToList());
        }

        // GET: Banner/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner tbl_PortalHaber_Banner = db.tbl_PortalHaber_Banner.Find(id);
            if (tbl_PortalHaber_Banner == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Banner);
        }

        // GET: Banner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Banner/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Bid,BannerName,IsEnabled")] tbl_PortalHaber_Banner tbl_PortalHaber_Banner)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_Banner.Add(tbl_PortalHaber_Banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_PortalHaber_Banner);
        }

        // GET: Banner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner tbl_PortalHaber_Banner = db.tbl_PortalHaber_Banner.Find(id);
            if (tbl_PortalHaber_Banner == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Banner);
        }

        // POST: Banner/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Bid,BannerName,IsEnabled")] tbl_PortalHaber_Banner tbl_PortalHaber_Banner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_Banner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_PortalHaber_Banner);
        }

        // GET: Banner/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner tbl_PortalHaber_Banner = db.tbl_PortalHaber_Banner.Find(id);
            if (tbl_PortalHaber_Banner == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Banner);
        }

        // POST: Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_Banner tbl_PortalHaber_Banner = db.tbl_PortalHaber_Banner.Find(id);
            db.tbl_PortalHaber_Banner.Remove(tbl_PortalHaber_Banner);
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
