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
    public class YazarParaTalepController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: YazarParaTalep
        public ActionResult Index()
        {
            var tbl_PortalHaber_YazarParaTalep = db.tbl_PortalHaber_YazarParaTalep.Include(t => t.tbl_PortalHaber_Yazar);
            return View(tbl_PortalHaber_YazarParaTalep.ToList());
        }

        // GET: YazarParaTalep/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_YazarParaTalep tbl_PortalHaber_YazarParaTalep = db.tbl_PortalHaber_YazarParaTalep.Find(id);
            if (tbl_PortalHaber_YazarParaTalep == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_YazarParaTalep);
        }

        // GET: YazarParaTalep/Create
        public ActionResult Create()
        {
            ViewBag.Yazar_Id = new SelectList(db.tbl_PortalHaber_Yazar, "YazarId", "YName");
            return View();
        }

        // POST: YazarParaTalep/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pt_id,Yazar_Id,Site_Grup_Id,Talep_Tarihi,Talep_Miktar,Onay_Tarihi,Onay_Miktar")] tbl_PortalHaber_YazarParaTalep tbl_PortalHaber_YazarParaTalep)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_YazarParaTalep.Add(tbl_PortalHaber_YazarParaTalep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Yazar_Id = new SelectList(db.tbl_PortalHaber_Yazar, "YazarId", "YName", tbl_PortalHaber_YazarParaTalep.Yazar_Id);
            return View(tbl_PortalHaber_YazarParaTalep);
        }

        // GET: YazarParaTalep/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_YazarParaTalep tbl_PortalHaber_YazarParaTalep = db.tbl_PortalHaber_YazarParaTalep.Find(id);
            if (tbl_PortalHaber_YazarParaTalep == null)
            {
                return HttpNotFound();
            }
            ViewBag.Yazar_Id = new SelectList(db.tbl_PortalHaber_Yazar, "YazarId", "YName", tbl_PortalHaber_YazarParaTalep.Yazar_Id);
            return View(tbl_PortalHaber_YazarParaTalep);
        }

        // POST: YazarParaTalep/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pt_id,Yazar_Id,Site_Grup_Id,Talep_Tarihi,Talep_Miktar,Onay_Tarihi,Onay_Miktar")] tbl_PortalHaber_YazarParaTalep tbl_PortalHaber_YazarParaTalep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_YazarParaTalep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Yazar_Id = new SelectList(db.tbl_PortalHaber_Yazar, "YazarId", "YName", tbl_PortalHaber_YazarParaTalep.Yazar_Id);
            return View(tbl_PortalHaber_YazarParaTalep);
        }

        // GET: YazarParaTalep/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_YazarParaTalep tbl_PortalHaber_YazarParaTalep = db.tbl_PortalHaber_YazarParaTalep.Find(id);
            if (tbl_PortalHaber_YazarParaTalep == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_YazarParaTalep);
        }

        // POST: YazarParaTalep/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_YazarParaTalep tbl_PortalHaber_YazarParaTalep = db.tbl_PortalHaber_YazarParaTalep.Find(id);
            db.tbl_PortalHaber_YazarParaTalep.Remove(tbl_PortalHaber_YazarParaTalep);
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
