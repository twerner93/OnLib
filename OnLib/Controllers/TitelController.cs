using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnLib.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnLib.Controllers
{
    [Authorize]
    public class TitelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        // GET: /Titel/
        public ActionResult Index(string typ)
        {
            List<Titel> titels = null;
            if (!String.IsNullOrEmpty(typ))
            {
                titels = db.Titels.Include(t => t.Autor).Include(t => t.Genre).Include(t => t.Typ)
                    .Where(t => t.Typ.Name == typ).OrderBy(t => t.Name).ToList();
            }
            else
            {
                titels = db.Titels.Include(t => t.Autor).Include(t => t.Genre)
                    .OrderBy(t => t.Name).Include(t => t.Typ).ToList();
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
                    CoverPfad = getCoverPath(item),
                    Erscheinung = item.Erscheinung,
                    Created = item.Created,
                    CreatedBy = item.CreatedBy,
                    Modified = item.Modified,
                    LastModifiedBy = item.LastModifiedBy,
                    Kopies = db.Kopies.Where(k => k.TitelId == item.TitelId).ToList()
                };
                titelviews.Add(tvm);
            }

            return View(titelviews);
        }

        [AllowAnonymous]
        public ActionResult Search(string searchString)
        {
            List<Titel> titels;
            List<TitelViewModel> titelviews;
            if (!String.IsNullOrEmpty(searchString))
            {
                titels = db.Titels.Include(t => t.Autor).Include(t => t.Genre).Include(t => t.Typ)
                    .Where(t => t.Name.Contains(searchString) || t.Autor.Vorname.Contains(searchString) ||
                    t.Autor.Nachname.Contains(searchString)).ToList();
            }
            else
            {
                return Index("");
            }

            titelviews = titelToTitelViewModel(titels);


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
                CoverPfad = getCoverPath(titel),
                Erscheinung = titel.Erscheinung,
                Created = titel.Created,
                CreatedBy = titel.CreatedBy,
                Modified = titel.Modified,
                LastModifiedBy = titel.LastModifiedBy,
                Kopies = db.Kopies.Where(k => k.TitelId == titel.TitelId).ToList()
            };
            foreach (Kopie item in titelview.Kopies)
            {
                List<Leihe> leihen = db.Leihes.Where(l => l.KopieId == item.Id).ToList();
                if (leihen.FirstOrDefault(l => l.KopieId == item.Id) == null)
                {
                    item.Available = true;
                }
                foreach (Leihe leihe in leihen)
                {
                    if (leihe.Zurueck == false)
                    {
                        item.Available = false;
                    }
                    else
                    {
                        item.Available = true;
                    }
                }
            }
            var currentUserId = User.Identity.GetUserId();
            ViewBag.currentUserId = currentUserId;
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
        public ActionResult Create([Bind(Include="TitelId,AutorNachname,AutorVorname,GenreName,TypId,Name,Kurzbeschreibung,Beschreibung,Erscheinung")] TitelViewModel titelview)
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
                if (!new GenreController().Exists(titelview.GenreName))
                {
                    Genre newGenre = new Genre
                    {
                        Name = titelview.GenreName
                    };
                    new GenreController().Create(newGenre);
                }
                Autor autor = db.Autors.FirstOrDefault(a => a.Nachname == titelview.AutorNachname && (String.IsNullOrEmpty(titelview.AutorVorname) || a.Vorname == titelview.AutorVorname));
                Typ typ = db.Typs.Where(t => t.TypId == titelview.TypId).Single();
                Genre genre = db.Genres.Where(g => g.Name == titelview.GenreName).Single();
                var currentUserId = User.Identity.GetUserId();
                if (Exists(titelview.Name, autor.AutorId))
                {
                    Titel temp1 = db.Titels.FirstOrDefault(t => t.Name == titelview.Name && t.AutorId == autor.AutorId);
                    return RedirectToAction("Create/" + temp1.TitelId, "Kopie");
                }
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
                    Erscheinung = titelview.Erscheinung,
                    Created = DateTime.Now,
                    CreatedBy = db.Users.Find(currentUserId),
                    Modified = DateTime.Now,
                    LastModifiedBy = db.Users.Find(currentUserId)
                };

                db.Titels.Add(titel);
                db.SaveChanges();

                Titel temp2 = db.Titels.FirstOrDefault(t => t.Name == titel.Name && t.AutorId == titel.AutorId);
                //return RedirectToAction("Index");
                //TODO RedirectToAction --> Cover-Upload
                return RedirectToAction("UploadCover", new { TitelId = temp2.TitelId });
                //return RedirectToAction("Create/" + temp2.TitelId, "Kopie");
            }

            //ViewBag.AutorId = new SelectList(db.Autors, "AutorId", "Nachname", titelview.AutorId);
            //ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", titelview.GenreId);
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
            titelview.GenreName = titel.Genre.Name;
            //ViewBag.AutorId = new SelectList(db.Autors, "AutorId", "Nachname", titel.AutorId);
            //ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", titel.GenreId);
            ViewBag.TypId = new SelectList(db.Typs, "TypId", "Name", titel.TypId);
            return View(titelview);
        }

        // POST: /Titel/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TitelId,AutorNachname,AutorVorname,GenreName,TypId,Name,Kurzbeschreibung,Beschreibung,Erscheinung")] TitelViewModel titelview)
        {
            if (ModelState.IsValid)
            {
                if (!new AutorController().Exists(titelview.AutorNachname, titelview.AutorVorname))
                {
                    Autor newAutor = new Autor
                    {
                        Nachname = titelview.AutorNachname
                    };
                    if (!String.IsNullOrEmpty(titelview.AutorVorname))
                    {
                        newAutor.Vorname = titelview.AutorVorname;
                    }
                    new AutorController().Create(newAutor);
                }

                if (!new GenreController().Exists(titelview.GenreName))
                {
                    Genre newGenre = new Genre
                    {
                        Name = titelview.GenreName
                    };

                    new GenreController().Create(newGenre);
                }

                Autor autor = db.Autors.Where(a => a.Nachname == titelview.AutorNachname && (String.IsNullOrEmpty(titelview.AutorVorname) || a.Vorname == titelview.AutorVorname)).Single();
                Typ typ = db.Typs.Where(t => t.TypId == titelview.TypId).Single();
                Genre genre = db.Genres.Where(g => g.Name == titelview.GenreName).Single();
                var currentUserId = User.Identity.GetUserId();

                Titel titel = db.Titels.Find(titelview.TitelId);
                titel.Autor = autor;
                titel.AutorId = autor.AutorId;
                titel.Name = titelview.Name;
                titel.Genre = genre;
                titel.GenreId = genre.GenreId;
                titel.Typ = typ;
                titel.Modified = DateTime.Now;
                titel.LastModifiedBy = db.Users.Find(currentUserId);
                if (!String.IsNullOrEmpty(titelview.Beschreibung)) { titel.Beschreibung = titelview.Beschreibung; }
                if (!String.IsNullOrEmpty(titelview.Kurzbeschreibung)) { titel.Kurzbeschreibung = titelview.Kurzbeschreibung; }
                titel.Erscheinung = titelview.Erscheinung;

                db.Entry(titel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", titelview.GenreId);
            ViewBag.TypId = new SelectList(db.Typs, "TypId", "Name", titelview.TypId);
            return View(titelview);
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

        // GET: /Titel/AddKopie/5
        public ActionResult AddKopie(int? id)
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
            return RedirectToAction("Create/"+titel.TitelId, "Kopie");
        }

        // GET: /Titel/UploadCover/5
        public ActionResult UploadCover(int TitelId)
        {
            CoverUploadModel model = new CoverUploadModel();
            model.TitelId = TitelId;
            model.Neu = true;
            return View(model);
        }

        // POST: /Titel/UploadCover/5
        [HttpPost]
        public ActionResult UploadCover(CoverUploadModel model)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    if (Request.Files[0].ContentLength > 0)
                    {
                        HttpPostedFileBase postedFile = Request.Files[0];
                        string filename = System.IO.Path.GetFileName(Request.Files[0].FileName);
                        string strLocation = HttpContext.Server.MapPath("~/images/titel");
                        Request.Files[0].SaveAs(strLocation + @"\" + filename.Replace('+', '_'));
                        Models.Titel titel = db.Titels.FirstOrDefault(t => t.TitelId == model.TitelId);
                        titel.CoverPfad = @"\images\titel\" + filename;

                        db.Entry(titel).State = EntityState.Modified;
                        db.SaveChanges();

                        if (model.Neu == true)
                        {
                            return RedirectToAction("Create/" + model.TitelId, "Kopie");
                        }
                        return RedirectToAction("Edit", new { id = model.TitelId});
                    }
                }
                catch (FormatException ex)
                {
                    return Content(ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("Edit", new { id = model.TitelId });
        }

        // GET: /Titel/MyMedia
        public ActionResult MyMedia()
        {
            var currentUserId = User.Identity.GetUserId();
            //TODO Benutzer holen und alle Titel des Benutzers
            List<Kopie> kopies = db.Kopies.Where(k => k.UserProfile.Id == currentUserId).ToList();
            List<KopieViewModel> titels = new List<KopieViewModel>();
            foreach (Kopie item in kopies)
            {
                Titel temp = db.Titels.Find(item.TitelId);
                KopieViewModel kopie = new KopieViewModel
                {
                    KopieId = item.Id,
                    TitelId = temp.TitelId,
                    TitelName = temp.Name,
                    AutorName = temp.Autor.Nachname,
                    Typ = item.Typ,
                    Ausgabe = item.Ausgabe,
                    Qualitaet = item.Qualitaet
                };
                kopie.CoverPfad = getCoverPath(temp);
                titels.Add(kopie);
            }
            return View(titels);
        }

        // GET: /Titel/Rents
        public ActionResult Rents()
        {
            var currentUserId = User.Identity.GetUserId();
            List<Leihe> rents = db.Leihes.Where(l => l.UserProfile.Id == currentUserId && l.Zurueck == false).ToList();
            foreach (Leihe item in rents)
            {
                item.Kopie = db.Kopies.Find(item.KopieId);
                item.Kopie.Titel = db.Titels.Find(item.Kopie.TitelId);
            }
            return View(rents);
        }

        // GET: /Titel/Back
        public ActionResult Back(int id)
        {
            Leihe rent = db.Leihes.Find(id);
            rent.Zurueck = true;
            db.SaveChanges();
            return RedirectToAction("Rents");
        }

        // GET: /Titel/Exists
        public bool Exists(string name, int autorid)
        {
            foreach (Titel item in db.Titels)
            {
                if (item.Name == name && item.AutorId == autorid)
                {
                    return true;
                }
            }
            return false;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AutocompleteSuggestions(string genre)
        {
            var suggestions = from g in db.Genres
                              select g.Name;
            var genrelist = suggestions.Where(g => g.ToLower().StartsWith(genre.ToLower()));
            return Json(genrelist, JsonRequestBehavior.AllowGet);
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
                    Erscheinung = titel.Erscheinung,
                    CoverPfad = getCoverPath(titel),
                    Created = titel.Created,
                    CreatedBy = titel.CreatedBy,
                    Modified = titel.Modified,
                    LastModifiedBy = titel.LastModifiedBy,
                    Kopies = db.Kopies.Where(k => k.TitelId == titel.TitelId).ToList()
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
                CoverPfad = getCoverPath(titel),
                Erscheinung = titel.Erscheinung,
                Created = titel.Created,
                CreatedBy = titel.CreatedBy,
                Modified = titel.Modified,
                LastModifiedBy = titel.LastModifiedBy,
                Kopies = db.Kopies.Where(k => k.TitelId == titel.TitelId).ToList()
            };
            return tvm;
        }

        public string getCoverPath(Titel titel)
        {
            if (titel.CoverPfad != null)
            {
                return titel.CoverPfad.Replace('\\', '/').Substring(1);
            }
            return null;                
        }

        public List<Titel> FuenfBeliebteste()
        {
            throw new NotImplementedException();
        }
    }
}
