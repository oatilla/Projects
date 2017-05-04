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
    public class Banner_VeriGelmeTipiController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: Banner_VeriGelmeTipi
        public ActionResult Index()
        {
            return View(db.tbl_PortalHaber_Banner_VeriGelmeTipi.ToList());
        }

        // GET: Banner_VeriGelmeTipi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner_VeriGelmeTipi tbl_PortalHaber_Banner_VeriGelmeTipi = db.tbl_PortalHaber_Banner_VeriGelmeTipi.Find(id);
            if (tbl_PortalHaber_Banner_VeriGelmeTipi == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Banner_VeriGelmeTipi);
        }

        // GET: Banner_VeriGelmeTipi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Banner_VeriGelmeTipi/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BT_Id,Adi")] tbl_PortalHaber_Banner_VeriGelmeTipi tbl_PortalHaber_Banner_VeriGelmeTipi)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_Banner_VeriGelmeTipi.Add(tbl_PortalHaber_Banner_VeriGelmeTipi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_PortalHaber_Banner_VeriGelmeTipi);
        }

        // GET: Banner_VeriGelmeTipi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner_VeriGelmeTipi tbl_PortalHaber_Banner_VeriGelmeTipi = db.tbl_PortalHaber_Banner_VeriGelmeTipi.Find(id);
            if (tbl_PortalHaber_Banner_VeriGelmeTipi == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Banner_VeriGelmeTipi);
        }

        // POST: Banner_VeriGelmeTipi/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BT_Id,Adi")] tbl_PortalHaber_Banner_VeriGelmeTipi tbl_PortalHaber_Banner_VeriGelmeTipi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_Banner_VeriGelmeTipi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_PortalHaber_Banner_VeriGelmeTipi);
        }

        // GET: Banner_VeriGelmeTipi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner_VeriGelmeTipi tbl_PortalHaber_Banner_VeriGelmeTipi = db.tbl_PortalHaber_Banner_VeriGelmeTipi.Find(id);
            if (tbl_PortalHaber_Banner_VeriGelmeTipi == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Banner_VeriGelmeTipi);
        }

        // POST: Banner_VeriGelmeTipi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_Banner_VeriGelmeTipi tbl_PortalHaber_Banner_VeriGelmeTipi = db.tbl_PortalHaber_Banner_VeriGelmeTipi.Find(id);
            db.tbl_PortalHaber_Banner_VeriGelmeTipi.Remove(tbl_PortalHaber_Banner_VeriGelmeTipi);
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
