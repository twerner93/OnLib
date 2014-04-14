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
    public class TitelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Titel/
        public ActionResult Index()
        {
            var titels = db.Titels.Include(t => t.Autor).Include(t => t.Genre).Include(t => t.Typ);
            return View(titels.ToList());
        }

        // GET: /Titel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titel titel = db.Titels.Find(id);
            if (titel == null)
            {
                return HttpNotFound();
            }
            return View(titel);
        }

        // GET: /Titel/Create
        public ActionResult Create()
        {
            ViewBag.AutorId = new SelectList(db.Autors, "AutorId", "Nachname");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            ViewBag.TypId = new SelectList(db.Typs, "TypId", "Name");
            return View();
        }

        // POST: /Titel/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TitelId,AutorId,GenreId,TypId,Name,Kurzbeschreibung,Beschreibung,Erscheinung")] Titel titel)
        {
            if (ModelState.IsValid)
            {
                db.Titels.Add(titel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AutorId = new SelectList(db.Autors, "AutorId", "Nachname", titel.AutorId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", titel.GenreId);
            ViewBag.TypId = new SelectList(db.Typs, "TypId", "Name", titel.TypId);
            return View(titel);
        }

        // GET: /Titel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titel titel = db.Titels.Find(id);
            if (titel == null)
            {
                return HttpNotFound();
            }
            ViewBag.AutorId = new SelectList(db.Autors, "AutorId", "Nachname", titel.AutorId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", titel.GenreId);
            ViewBag.TypId = new SelectList(db.Typs, "TypId", "Name", titel.TypId);
            return View(titel);
        }

        // POST: /Titel/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TitelId,AutorId,TypId,Name,Kurzbeschreibung,Beschreibung,Genre,Erscheinung")] Titel titel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(titel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AutorId = new SelectList(db.Autors, "AutorId", "Nachname", titel.AutorId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", titel.GenreId);
            ViewBag.TypId = new SelectList(db.Typs, "TypId", "Name", titel.TypId);
            return View(titel);
        }

        // GET: /Titel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Titel titel = db.Titels.Find(id);
            if (titel == null)
            {
                return HttpNotFound();
            }
            return View(titel);
        }

        // POST: /Titel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Titel titel = db.Titels.Find(id);
            db.Titels.Remove(titel);
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
