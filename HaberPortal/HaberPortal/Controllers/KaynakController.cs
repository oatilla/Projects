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
    public class KaynakController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: Kaynak
        public ActionResult Index()
        {
            return View(db.tbl_PortalHaber_Kaynak.ToList());
        }

        // GET: Kaynak/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Kaynak tbl_PortalHaber_Kaynak = db.tbl_PortalHaber_Kaynak.Find(id);
            if (tbl_PortalHaber_Kaynak == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Kaynak);
        }

        // GET: Kaynak/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kaynak/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KaynakId,Adi")] tbl_PortalHaber_Kaynak tbl_PortalHaber_Kaynak)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_Kaynak.Add(tbl_PortalHaber_Kaynak);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_PortalHaber_Kaynak);
        }

        // GET: Kaynak/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Kaynak tbl_PortalHaber_Kaynak = db.tbl_PortalHaber_Kaynak.Find(id);
            if (tbl_PortalHaber_Kaynak == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Kaynak);
        }

        // POST: Kaynak/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KaynakId,Adi")] tbl_PortalHaber_Kaynak tbl_PortalHaber_Kaynak)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_Kaynak).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_PortalHaber_Kaynak);
        }

        // GET: Kaynak/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Kaynak tbl_PortalHaber_Kaynak = db.tbl_PortalHaber_Kaynak.Find(id);
            if (tbl_PortalHaber_Kaynak == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Kaynak);
        }

        // POST: Kaynak/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_Kaynak tbl_PortalHaber_Kaynak = db.tbl_PortalHaber_Kaynak.Find(id);
            db.tbl_PortalHaber_Kaynak.Remove(tbl_PortalHaber_Kaynak);
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
