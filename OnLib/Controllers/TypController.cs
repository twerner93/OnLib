using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnLib.Models;

namespace OnLib.Controllers
{
    public class TypController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Typ/
        public ActionResult Index()
        {
            return View(db.Typs.ToList());
        }

        // GET: /Typ/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Typ typ = db.Typs.Find(id);
            if (typ == null)
            {
                return HttpNotFound();
            }
            return View(typ);
        }

        // GET: /Typ/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Typ/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TypId,Name,Beschreibung")] Typ typ)
        {
            if (ModelState.IsValid)
            {
                db.Typs.Add(typ);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typ);
        }

        // GET: /Typ/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Typ typ = db.Typs.Find(id);
            if (typ == null)
            {
                return HttpNotFound();
            }
            return View(typ);
        }

        // POST: /Typ/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TypId,Name,Beschreibung")] Typ typ)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typ);
        }

        // GET: /Typ/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Typ typ = db.Typs.Find(id);
            if (typ == null)
            {
                return HttpNotFound();
            }
            return View(typ);
        }

        // POST: /Typ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Typ typ = db.Typs.Find(id);
            db.Typs.Remove(typ);
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
