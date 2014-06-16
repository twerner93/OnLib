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
using System.Data.SqlTypes;

namespace OnLib.Controllers
{
    [Authorize]
    public class KopieController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Kopie/
        public ActionResult Index()
        {
            var kopies = db.Kopies.Include(k => k.Titel);
            var users = db.Users.Include(u => u.UserName);
            return View(kopies.ToList());
        }

        // GET: /Kopie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kopie kopie = db.Kopies.Find(id);
            if (kopie == null)
            {
                return HttpNotFound();
            }
            return View(kopie);
        }

        // GET: /Kopie/Create/5
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewBag.TitelId = new SelectList(db.Titels, "TitelId", "Name");
                ViewBag.forTitel = false;
                return View();
            }

            Titel titel = db.Titels.Find(id);
            Kopie kopie = new Kopie { TitelId = titel.TitelId, Titel = titel};

            ViewBag.forTitel = true;

            return View(kopie);
        }

        // POST: /Kopie/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,TitelId,Typ,Ausgabe,Qualitaet")] Kopie kopie)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                kopie.UserProfile = db.Users.Find(currentUserId);
                db.Kopies.Add(kopie);
                db.SaveChanges();
                return RedirectToAction("Details/" + kopie.TitelId, "Titel");
            }

            ViewBag.TitelId = new SelectList(db.Titels, "TitelId", "Name", kopie.TitelId);
            return View(kopie);
        }

        // GET: /Kopie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kopie kopie = db.Kopies.Find(id);
            if (kopie == null)
            {
                return HttpNotFound();
            }
            ViewBag.TitelId = new SelectList(db.Titels, "TitelId", "Name", kopie.TitelId);
            return View(kopie);
        }

        // POST: /Kopie/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,TitelId,Typ,Ausgabe,Qualitaet")] Kopie kopie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kopie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TitelId = new SelectList(db.Titels, "TitelId", "Name", kopie.TitelId);
            return View(kopie);
        }

        // GET: /Kopie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kopie kopie = db.Kopies.Find(id);
            if (kopie == null)
            {
                return HttpNotFound();
            }
            return View(kopie);
        }

        // POST: /Kopie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kopie kopie = db.Kopies.Find(id);
            db.Kopies.Remove(kopie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /Kopie/Leihen/5
        public ActionResult Leihen(int? id)
        {
            if (id == null)
            {
                ViewBag.KopieId = new SelectList(db.Kopies, "KopieId", "Name");
                ViewBag.forKopie = false;
                return View();
            }
            Kopie kopie = db.Kopies.Find(id);
            var currentUserId = User.Identity.GetUserId();
            Leihe leihe = new Leihe
            {
                Kopie = kopie,
                KopieId = kopie.Id,
                UserProfile = db.Users.Find(currentUserId),
                Ausgeliehen = SqlDateTime.MinValue.Value
            };
            ViewBag.ausgeliehen = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            List<RueckgabeDatum> dauer = getDauerList();
            ViewBag.rueckgabe = new SelectList(dauer, "Datum", "Bezeichner");
            return View(leihe);
        }

        // POST: /Kopie/Leihen/5
        [HttpPost, ActionName("Leihen")]
        [ValidateAntiForgeryToken]
        public ActionResult Leihen(Leihe leihe)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                leihe.Kopie = db.Kopies.FirstOrDefault(k => k.Id == leihe.KopieId);
                leihe.UserProfile = db.Users.Find(currentUserId);
                leihe.Zurueck = false;
                db.Leihes.Add(leihe);
                db.SaveChanges();
                return RedirectToAction("Details/" + leihe.Kopie.TitelId, "Titel");
            }
            return View(leihe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<RueckgabeDatum> getDauerList()
        {
            List<RueckgabeDatum> dauer = new List<RueckgabeDatum>();
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dauer.Add(new RueckgabeDatum{Bezeichner = "1 Tag", Datum = now.AddDays(1)});
            dauer.Add(new RueckgabeDatum{Bezeichner = "2 Tage", Datum = now.AddDays(2)});
            dauer.Add(new RueckgabeDatum{Bezeichner = "3 Tage", Datum = now.AddDays(3)});
            dauer.Add(new RueckgabeDatum{Bezeichner = "1 Woche", Datum = now.AddDays(7)});
            dauer.Add(new RueckgabeDatum{Bezeichner = "2 Wochen", Datum = now.AddDays(14)});
            dauer.Add(new RueckgabeDatum{Bezeichner = "1 Monat", Datum = now.AddMonths(1)});
            return dauer;
        }

        class RueckgabeDatum
        {
            public string Bezeichner { get; set; }
            public DateTime Datum { get; set; }
        }
    }
}
