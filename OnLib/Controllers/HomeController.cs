using OnLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnLib.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var model = _db.Titels.ToList();

            List<Titel> titels = _db.Titels.OrderByDescending(t => t.Created).ToList();
            List<Titel> neuste = new List<Titel>();
            List<Titel> beliebteste = new List<Titel>();
            for(int i = 0; i <5; i++){
                neuste.Add(titels[i]);
            }
            titels = _db.Titels.OrderBy(t => t.Created).ToList();
            for (int i = 0; i < 5; i++)
            {
                beliebteste.Add(titels[i]);
            }
            ViewBag.Neuste = neuste;
            ViewBag.Beliebteste = beliebteste;
            

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}