using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class KopieViewModel
    {
        public int KopieId { get; set; }
        public int TitelId { get; set; }
        public string Typ { get; set; }
        public string Ausgabe { get; set; }
        public string Qualitaet { get; set; }
        public virtual bool Available { get; set; }

        public virtual Titel Titel { get; set; }
        public virtual ApplicationUser UserProfile { get; set; }



    }
}