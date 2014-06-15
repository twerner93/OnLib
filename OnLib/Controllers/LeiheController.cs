using OnLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnLib.Controllers
{
    [Authorize]
    public class LeiheController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        //
        // GET: /Leihe/
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            ApplicationUser UserProfile = db.Users.Find(currentUserId);

            List<Leihe> rents = db.Leihes.Where(l => l.UserProfile.Id == UserProfile.Id).ToList();

            return View();
        }

        //
        // GET: /Leihe/Details/5
        public ActionResult Details(int id)
        {
            Leihe leihe = db.Leihes.Find(id);

            return View(leihe);
        }

        //
        // GET: /Leihe/Create/5
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View();
            }
            //id ist Kopie Id
            Kopie kopie = db.Kopies.Find(id);
            if (kopie == null)
            {
                return HttpNotFound();
            }
            Leihe leihe  = new Leihe
            {
                Kopie = kopie,
                KopieId = kopie.Id
            };
            return View(leihe);
        }

        //
        // POST: /Leihe/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Leihe/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Leihe/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Leihe/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Leihe/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
