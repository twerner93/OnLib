using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class KopieViewModel
    {
        public int KopieId { get; set; }
        public int TitelId { get; set; }

        [Display(Name = "Name")]
        public string TitelName { get; set; }
        [Display(Name="Autor")]
        public string AutorName { get; set; }
        public string Typ { get; set; }
        public string Ausgabe { get; set; }
        public string Qualitaet { get; set; }
        public string CoverPfad { get; set; }
        public virtual bool Available { get; set; }

        public virtual Titel Titel { get; set; }
        public virtual ApplicationUser UserProfile { get; set; }



    }
}