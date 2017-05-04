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
    public class YazarParaAyarController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: YazarParaAyar
        public ActionResult Index()
        {
            return View(db.tbl_PortalHaber_YazarParaAyar.ToList());
        }

        // GET: YazarParaAyar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_YazarParaAyar tbl_PortalHaber_YazarParaAyar = db.tbl_PortalHaber_YazarParaAyar.Find(id);
            if (tbl_PortalHaber_YazarParaAyar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_YazarParaAyar);
        }

        // GET: YazarParaAyar/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: YazarParaAyar/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YPid,Para,Tarih,SiteGrupId")] tbl_PortalHaber_YazarParaAyar tbl_PortalHaber_YazarParaAyar)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_YazarParaAyar.Add(tbl_PortalHaber_YazarParaAyar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_PortalHaber_YazarParaAyar);
        }

        // GET: YazarParaAyar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_YazarParaAyar tbl_PortalHaber_YazarParaAyar = db.tbl_PortalHaber_YazarParaAyar.Find(id);
            if (tbl_PortalHaber_YazarParaAyar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_YazarParaAyar);
        }

        // POST: YazarParaAyar/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YPid,Para,Tarih,SiteGrupId")] tbl_PortalHaber_YazarParaAyar tbl_PortalHaber_YazarParaAyar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_YazarParaAyar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_PortalHaber_YazarParaAyar);
        }

        // GET: YazarParaAyar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_YazarParaAyar tbl_PortalHaber_YazarParaAyar = db.tbl_PortalHaber_YazarParaAyar.Find(id);
            if (tbl_PortalHaber_YazarParaAyar == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_YazarParaAyar);
        }

        // POST: YazarParaAyar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_YazarParaAyar tbl_PortalHaber_YazarParaAyar = db.tbl_PortalHaber_YazarParaAyar.Find(id);
            db.tbl_PortalHaber_YazarParaAyar.Remove(tbl_PortalHaber_YazarParaAyar);
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
