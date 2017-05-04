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
    public class Menu_Tip_RelationController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: Menu_Tip_Relation
        public ActionResult Index()
        {
            var tbl_PortalHaber_Menu_Tip_Relation = db.tbl_PortalHaber_Menu_Tip_Relation.Include(t => t.tbl_PortalHaber_Menu).Include(t => t.tbl_PortalHaber_Tip);
            return View(tbl_PortalHaber_Menu_Tip_Relation.ToList());
        }

        // GET: Menu_Tip_Relation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Menu_Tip_Relation tbl_PortalHaber_Menu_Tip_Relation = db.tbl_PortalHaber_Menu_Tip_Relation.Find(id);
            if (tbl_PortalHaber_Menu_Tip_Relation == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Menu_Tip_Relation);
        }

        // GET: Menu_Tip_Relation/Create
        public ActionResult Create()
        {
            ViewBag.MenuId = new SelectList(db.tbl_PortalHaber_Menu, "Id", "MenuAciklama");
            ViewBag.TipId = new SelectList(db.tbl_PortalHaber_Tip, "Tip_Id", "Tip_Adi");
            return View();
        }

        // POST: Menu_Tip_Relation/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipId,MenuId")] tbl_PortalHaber_Menu_Tip_Relation tbl_PortalHaber_Menu_Tip_Relation)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_Menu_Tip_Relation.Add(tbl_PortalHaber_Menu_Tip_Relation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MenuId = new SelectList(db.tbl_PortalHaber_Menu, "Id", "MenuAciklama", tbl_PortalHaber_Menu_Tip_Relation.MenuId);
            ViewBag.TipId = new SelectList(db.tbl_PortalHaber_Tip, "Tip_Id", "Tip_Adi", tbl_PortalHaber_Menu_Tip_Relation.TipId);
            return View(tbl_PortalHaber_Menu_Tip_Relation);
        }

        // GET: Menu_Tip_Relation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Menu_Tip_Relation tbl_PortalHaber_Menu_Tip_Relation = db.tbl_PortalHaber_Menu_Tip_Relation.Find(id);
            if (tbl_PortalHaber_Menu_Tip_Relation == null)
            {
                return HttpNotFound();
            }
            ViewBag.MenuId = new SelectList(db.tbl_PortalHaber_Menu, "Id", "MenuAciklama", tbl_PortalHaber_Menu_Tip_Relation.MenuId);
            ViewBag.TipId = new SelectList(db.tbl_PortalHaber_Tip, "Tip_Id", "Tip_Adi", tbl_PortalHaber_Menu_Tip_Relation.TipId);
            return View(tbl_PortalHaber_Menu_Tip_Relation);
        }

        // POST: Menu_Tip_Relation/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipId,MenuId")] tbl_PortalHaber_Menu_Tip_Relation tbl_PortalHaber_Menu_Tip_Relation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_Menu_Tip_Relation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MenuId = new SelectList(db.tbl_PortalHaber_Menu, "Id", "MenuAciklama", tbl_PortalHaber_Menu_Tip_Relation.MenuId);
            ViewBag.TipId = new SelectList(db.tbl_PortalHaber_Tip, "Tip_Id", "Tip_Adi", tbl_PortalHaber_Menu_Tip_Relation.TipId);
            return View(tbl_PortalHaber_Menu_Tip_Relation);
        }

        // GET: Menu_Tip_Relation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Menu_Tip_Relation tbl_PortalHaber_Menu_Tip_Relation = db.tbl_PortalHaber_Menu_Tip_Relation.Find(id);
            if (tbl_PortalHaber_Menu_Tip_Relation == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Menu_Tip_Relation);
        }

        // POST: Menu_Tip_Relation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_Menu_Tip_Relation tbl_PortalHaber_Menu_Tip_Relation = db.tbl_PortalHaber_Menu_Tip_Relation.Find(id);
            db.tbl_PortalHaber_Menu_Tip_Relation.Remove(tbl_PortalHaber_Menu_Tip_Relation);
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
