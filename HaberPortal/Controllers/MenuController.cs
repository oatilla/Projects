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
    public class MenuController : Controller
    {
        private DB090928093827Entities db = new DB090928093827Entities();

        // GET: Menu
        public ActionResult Index()
        {
            return View(db.tbl_PortalHaber_Menu.ToList());
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Menu tbl_PortalHaber_Menu = db.tbl_PortalHaber_Menu.Find(id);
            if (tbl_PortalHaber_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Menu);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MenuAciklama,MenuLink,Sıra")] tbl_PortalHaber_Menu tbl_PortalHaber_Menu)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PortalHaber_Menu.Add(tbl_PortalHaber_Menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_PortalHaber_Menu);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Menu tbl_PortalHaber_Menu = db.tbl_PortalHaber_Menu.Find(id);
            if (tbl_PortalHaber_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Menu);
        }

        // POST: Menu/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MenuAciklama,MenuLink,Sıra")] tbl_PortalHaber_Menu tbl_PortalHaber_Menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PortalHaber_Menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_PortalHaber_Menu);
        }

        // GET: Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PortalHaber_Menu tbl_PortalHaber_Menu = db.tbl_PortalHaber_Menu.Find(id);
            if (tbl_PortalHaber_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PortalHaber_Menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_PortalHaber_Menu tbl_PortalHaber_Menu = db.tbl_PortalHaber_Menu.Find(id);
            db.tbl_PortalHaber_Menu.Remove(tbl_PortalHaber_Menu);
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
