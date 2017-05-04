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
    public class kategoriController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: kategori
        public ActionResult Index()
        {
            return View(db.tbl_PortalHaber_kategori.ToList());
        }

        // GET: kategori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_kategori tbl_PortalHaber_kategori = db.tbl_PortalHaber_kategori.Find(id);
            if (tbl_PortalHaber_kategori == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_kategori);
        }

        // GET: kategori/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: kategori/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Kid,KName")] tbl_PortalHaber_kategori tbl_PortalHaber_kategori)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_kategori.Add(tbl_PortalHaber_kategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_PortalHaber_kategori);
        }

        // GET: kategori/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_kategori tbl_PortalHaber_kategori = db.tbl_PortalHaber_kategori.Find(id);
            if (tbl_PortalHaber_kategori == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_kategori);
        }

        // POST: kategori/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Kid,KName")] tbl_PortalHaber_kategori tbl_PortalHaber_kategori)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_kategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_PortalHaber_kategori);
        }

        // GET: kategori/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_kategori tbl_PortalHaber_kategori = db.tbl_PortalHaber_kategori.Find(id);
            if (tbl_PortalHaber_kategori == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_kategori);
        }

        // POST: kategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_kategori tbl_PortalHaber_kategori = db.tbl_PortalHaber_kategori.Find(id);
            db.tbl_PortalHaber_kategori.Remove(tbl_PortalHaber_kategori);
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
