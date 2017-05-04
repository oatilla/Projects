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
    public class IPController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: IP
        public ActionResult Index()
        {
            var tbl_PortalHaber_IP = db.tbl_PortalHaber_IP.Include(t => t.tbl_PortalHaber_Icerik);
            return View(tbl_PortalHaber_IP.ToList());
        }

        // GET: IP/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_IP tbl_PortalHaber_IP = db.tbl_PortalHaber_IP.Find(id);
            if (tbl_PortalHaber_IP == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_IP);
        }

        // GET: IP/Create
        public ActionResult Create()
        {
            ViewBag.Haber_Id = new SelectList(db.tbl_PortalHaber_Icerik, "HaberId", "HaberBaslik");
            return View();
        }

        // POST: IP/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IP_ID,Haber_Id,IP")] tbl_PortalHaber_IP tbl_PortalHaber_IP)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_IP.Add(tbl_PortalHaber_IP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Haber_Id = new SelectList(db.tbl_PortalHaber_Icerik, "HaberId", "HaberBaslik", tbl_PortalHaber_IP.Haber_Id);
            return View(tbl_PortalHaber_IP);
        }

        // GET: IP/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_IP tbl_PortalHaber_IP = db.tbl_PortalHaber_IP.Find(id);
            if (tbl_PortalHaber_IP == null)
            {
                return HttpNotFound();
            }
            ViewBag.Haber_Id = new SelectList(db.tbl_PortalHaber_Icerik, "HaberId", "HaberBaslik", tbl_PortalHaber_IP.Haber_Id);
            return View(tbl_PortalHaber_IP);
        }

        // POST: IP/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IP_ID,Haber_Id,IP")] tbl_PortalHaber_IP tbl_PortalHaber_IP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_IP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Haber_Id = new SelectList(db.tbl_PortalHaber_Icerik, "HaberId", "HaberBaslik", tbl_PortalHaber_IP.Haber_Id);
            return View(tbl_PortalHaber_IP);
        }

        // GET: IP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_IP tbl_PortalHaber_IP = db.tbl_PortalHaber_IP.Find(id);
            if (tbl_PortalHaber_IP == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_IP);
        }

        // POST: IP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_IP tbl_PortalHaber_IP = db.tbl_PortalHaber_IP.Find(id);
            db.tbl_PortalHaber_IP.Remove(tbl_PortalHaber_IP);
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
