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
    public class Banner_Site_RelationController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: Banner_Site_Relation
        public ActionResult Index()
        {
            var tbl_PortalHaber_Banner_Site_Relation = db.tbl_PortalHaber_Banner_Site_Relation.Include(t => t.tbl_PortalHaber_Banner).Include(t => t.tbl_PortalHaber_Banner_VeriGelmeTipi).Include(t => t.tbl_PortalHaber_Site);
            return View(tbl_PortalHaber_Banner_Site_Relation.ToList());
        }

        // GET: Banner_Site_Relation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner_Site_Relation tbl_PortalHaber_Banner_Site_Relation = db.tbl_PortalHaber_Banner_Site_Relation.Find(id);
            if (tbl_PortalHaber_Banner_Site_Relation == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Banner_Site_Relation);
        }

        // GET: Banner_Site_Relation/Create
        public ActionResult Create()
        {
            ViewBag.BannerId = new SelectList(db.tbl_PortalHaber_Banner, "Bid", "BannerName");
            ViewBag.BTId = new SelectList(db.tbl_PortalHaber_Banner_VeriGelmeTipi, "BT_Id", "Adi");
            ViewBag.SiteId = new SelectList(db.tbl_PortalHaber_Site, "Site_Id", "Site_Adi");
            return View();
        }

        // POST: Banner_Site_Relation/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SB_Id,SiteId,BannerId,BTId,BannerCount")] tbl_PortalHaber_Banner_Site_Relation tbl_PortalHaber_Banner_Site_Relation)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_Banner_Site_Relation.Add(tbl_PortalHaber_Banner_Site_Relation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BannerId = new SelectList(db.tbl_PortalHaber_Banner, "Bid", "BannerName", tbl_PortalHaber_Banner_Site_Relation.BannerId);
            ViewBag.BTId = new SelectList(db.tbl_PortalHaber_Banner_VeriGelmeTipi, "BT_Id", "Adi", tbl_PortalHaber_Banner_Site_Relation.BTId);
            ViewBag.SiteId = new SelectList(db.tbl_PortalHaber_Site, "Site_Id", "Site_Adi", tbl_PortalHaber_Banner_Site_Relation.SiteId);
            return View(tbl_PortalHaber_Banner_Site_Relation);
        }

        // GET: Banner_Site_Relation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner_Site_Relation tbl_PortalHaber_Banner_Site_Relation = db.tbl_PortalHaber_Banner_Site_Relation.Find(id);
            if (tbl_PortalHaber_Banner_Site_Relation == null)
            {
                return HttpNotFound();
            }
            ViewBag.BannerId = new SelectList(db.tbl_PortalHaber_Banner, "Bid", "BannerName", tbl_PortalHaber_Banner_Site_Relation.BannerId);
            ViewBag.BTId = new SelectList(db.tbl_PortalHaber_Banner_VeriGelmeTipi, "BT_Id", "Adi", tbl_PortalHaber_Banner_Site_Relation.BTId);
            ViewBag.SiteId = new SelectList(db.tbl_PortalHaber_Site, "Site_Id", "Site_Adi", tbl_PortalHaber_Banner_Site_Relation.SiteId);
            return View(tbl_PortalHaber_Banner_Site_Relation);
        }

        // POST: Banner_Site_Relation/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SB_Id,SiteId,BannerId,BTId,BannerCount")] tbl_PortalHaber_Banner_Site_Relation tbl_PortalHaber_Banner_Site_Relation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_Banner_Site_Relation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BannerId = new SelectList(db.tbl_PortalHaber_Banner, "Bid", "BannerName", tbl_PortalHaber_Banner_Site_Relation.BannerId);
            ViewBag.BTId = new SelectList(db.tbl_PortalHaber_Banner_VeriGelmeTipi, "BT_Id", "Adi", tbl_PortalHaber_Banner_Site_Relation.BTId);
            ViewBag.SiteId = new SelectList(db.tbl_PortalHaber_Site, "Site_Id", "Site_Adi", tbl_PortalHaber_Banner_Site_Relation.SiteId);
            return View(tbl_PortalHaber_Banner_Site_Relation);
        }

        // GET: Banner_Site_Relation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Banner_Site_Relation tbl_PortalHaber_Banner_Site_Relation = db.tbl_PortalHaber_Banner_Site_Relation.Find(id);
            if (tbl_PortalHaber_Banner_Site_Relation == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Banner_Site_Relation);
        }

        // POST: Banner_Site_Relation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_Banner_Site_Relation tbl_PortalHaber_Banner_Site_Relation = db.tbl_PortalHaber_Banner_Site_Relation.Find(id);
            db.tbl_PortalHaber_Banner_Site_Relation.Remove(tbl_PortalHaber_Banner_Site_Relation);
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
