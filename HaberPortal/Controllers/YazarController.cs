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
    public class YazarController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: Yazar
        public ActionResult Index()
        {
            var tbl_PortalHaber_Yazar = db.tbl_PortalHaber_Yazar.Include(t => t.tbl_PortalHaber_Tip);
            return View(tbl_PortalHaber_Yazar.ToList());
        }

        // GET: Yazar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Yazar tbl_PortalHaber_Yazar = db.tbl_PortalHaber_Yazar.Find(id);
            if (tbl_PortalHaber_Yazar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Yazar);
        }

        // GET: Yazar/Create
        public ActionResult Create()
        {
            ViewBag.Tipi = new SelectList(db.tbl_PortalHaber_Tip, "Tip_Id", "Tip_Adi");
            return View();
        }

        // POST: Yazar/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YazarId,YName,Tipi,Sifre,EPosta,Resim")] tbl_PortalHaber_Yazar tbl_PortalHaber_Yazar)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_Yazar.Add(tbl_PortalHaber_Yazar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Tipi = new SelectList(db.tbl_PortalHaber_Tip, "Tip_Id", "Tip_Adi", tbl_PortalHaber_Yazar.Tipi);
            return View(tbl_PortalHaber_Yazar);
        }

        // GET: Yazar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Yazar tbl_PortalHaber_Yazar = db.tbl_PortalHaber_Yazar.Find(id);
            if (tbl_PortalHaber_Yazar == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tipi = new SelectList(db.tbl_PortalHaber_Tip, "Tip_Id", "Tip_Adi", tbl_PortalHaber_Yazar.Tipi);
            return View(tbl_PortalHaber_Yazar);
        }

        // POST: Yazar/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YazarId,YName,Tipi,Sifre,EPosta,Resim")] tbl_PortalHaber_Yazar tbl_PortalHaber_Yazar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_Yazar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Tipi = new SelectList(db.tbl_PortalHaber_Tip, "Tip_Id", "Tip_Adi", tbl_PortalHaber_Yazar.Tipi);
            return View(tbl_PortalHaber_Yazar);
        }

        // GET: Yazar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Yazar tbl_PortalHaber_Yazar = db.tbl_PortalHaber_Yazar.Find(id);
            if (tbl_PortalHaber_Yazar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Yazar);
        }

        // POST: Yazar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_Yazar tbl_PortalHaber_Yazar = db.tbl_PortalHaber_Yazar.Find(id);
            db.tbl_PortalHaber_Yazar.Remove(tbl_PortalHaber_Yazar);
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
