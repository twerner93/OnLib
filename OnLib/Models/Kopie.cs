using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class Kopie
    {
        public int Id { get; set; }
        public int TitelId { get; set; }
        public string Typ { get; set; }
        public string Ausgabe { get; set; }
        public string Qualitaet { get; set; }

        public virtual Titel Titel { get; set; }
        public virtual ApplicationUser UserProfile { get; set; }
        public virtual ICollection<Leihe> Leihes { get; set; }
    }
}