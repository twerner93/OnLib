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
        public ActionResult Index(string typ)
        {
            List<Titel> titels = null;
            if (!String.IsNullOrEmpty(typ))
            {
                titels = db.Titels.Include(t => t.Autor).Include(t => t.Genre).Include(t => t.Typ).Where(t => t.Typ.Name == typ).ToList();
            }
            else
            {
                titels = db.Titels.Include(t => t.Autor).Include(t => t.Genre).Include(t => t.Typ).ToList();
            }
            List<TitelViewModel> titelviews = new List<TitelViewModel>();
            foreach (Titel item in titels)
            {
                TitelViewModel tvm = new TitelViewModel
                {
                    TitelId = item.TitelId,
                    Autor = item.Autor,
                    Genre = item.Genre,
                    Typ = item.Typ,
                    Name = item.Name,
                    Kurzbeschreibung = item.Kurzbeschreibung,
                    Beschreibung = item.Beschreibung,
                    Erscheinung = item.Erscheinung
                };
                titelviews.Add(tvm);
            }

            return View(titelviews);
        }
                
        // GET: /Titel/ByTyp/string
        public ActionResult ByTyp(string typ)
        {
            //string bla = "";
            //bla += "controller: " + RouteData.Values["controller"] + "\n";
            //bla += "action: " + RouteData.Values["action"] + "\n";
            //bla += "queryString: " + Request.Url.Query + "\t";
            //bla += "typ: " + RouteData.Values["typ"];
            //bla += "typ: " + Request.QueryString["typ"];


            //return typ;
            
            var titels = db.Titels
                .Include(t => t.Autor)
                .Include(t => t.Genre)
                .Include(t => t.Typ)
                .Where(t => t.Typ.Name == typ);

            List<TitelViewModel> titelviews = titelToTitelViewModel(titels);
            return View(titelviews);
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
            TitelViewModel titelview = new TitelViewModel
            {
                TitelId = titel.TitelId,
                Autor = titel.Autor,
                Genre = titel.Genre,
                Typ = titel.Typ,
                Name = titel.Name,
                Kurzbeschreibung = titel.Kurzbeschreibung,
                Beschreibung = titel.Beschreibung,
                Erscheinung = titel.Erscheinung
            };
            return View(titelview);
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
        public ActionResult Create([Bind(Include="TitelId,AutorNachname,AutorVorname,GenreId,TypId,Name,Kurzbeschreibung,Beschreibung,Erscheinung")] TitelViewModel titelview)
        {
            if (ModelState.IsValid)
            {
                if (!new AutorController().Exists(titelview.AutorNachname, titelview.AutorVorname))
                {
                    Autor newAutor = new Autor
                    {
                        Nachname = titelview.AutorNachname,

                    };
                    if(!String.IsNullOrEmpty(titelview.AutorVorname)){
                        newAutor.Vorname = titelview.AutorVorname;
                    }
                    new AutorController().Create(newAutor);
                }
                Autor autor = db.Autors.Where(a => a.Nachname == titelview.AutorNachname && (String.IsNullOrEmpty(titelview.AutorVorname) || a.Vorname == titelview.AutorVorname)).Single();
                Typ typ = db.Typs.Where(t => t.TypId == titelview.TypId).Single();
                Genre genre = db.Genres.Where(g => g.GenreId == titelview.GenreId).Single();

                Titel titel = new Titel
                {
                    AutorId = autor.AutorId,
                    Autor = autor,
                    TypId = typ.TypId,
                    Typ = typ,
                    GenreId = genre.GenreId,
                    Genre = genre,
                    Name = titelview.Name,
                    Kurzbeschreibung = titelview.Kurzbeschreibung,
                    Beschreibung = titelview.Beschreibung,
                    Erscheinung = titelview.Erscheinung
                };

                db.Titels.Add(titel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.AutorId = new SelectList(db.Autors, "AutorId", "Nachname", titelview.AutorId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", titelview.GenreId);
            ViewBag.TypId = new SelectList(db.Typs, "TypId", "Name", titelview.TypId);
            return View(titelview);
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

            //toTitelViewModel
            TitelViewModel titelview = titelToTitelViewModel(titel);
            titelview.AutorNachname = titel.Autor.Nachname;
            if(!String.IsNullOrEmpty(titel.Autor.Vorname))
            {
                titelview.AutorVorname = titel.Autor.Vorname;
            }
            //ViewBag.AutorId = new SelectList(db.Autors, "AutorId", "Nachname", titel.AutorId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", titel.GenreId);
            ViewBag.TypId = new SelectList(db.Typs, "TypId", "Name", titel.TypId);
            return View(titelview);
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


        private List<TitelViewModel> titelToTitelViewModel(IEnumerable<Titel> titels)
        {
            List<TitelViewModel> titelviews = new List<TitelViewModel>();
            foreach (Titel titel in titels)
            {
                TitelViewModel tvm = new TitelViewModel
                {
                    TitelId = titel.TitelId,
                    Autor = titel.Autor,
                    Genre = titel.Genre,
                    Typ = titel.Typ,
                    Name = titel.Name,
                    Kurzbeschreibung = titel.Kurzbeschreibung,
                    Beschreibung = titel.Beschreibung,
                    Erscheinung = titel.Erscheinung
                };
                titelviews.Add(tvm);
            }
            return titelviews;
        }


        protected TitelViewModel titelToTitelViewModel(Titel titel)
        {
            TitelViewModel tvm = new TitelViewModel
            {
                TitelId = titel.TitelId,
                Autor = titel.Autor,
                Genre = titel.Genre,
                Typ = titel.Typ,
                Name = titel.Name,
                Kurzbeschreibung = titel.Kurzbeschreibung,
                Beschreibung = titel.Beschreibung,
                Erscheinung = titel.Erscheinung
            };
            return tvm;
        }
    }
}
