using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class Leihe
    {
        public int LeiheId { get; set; }
        public int KopieId { get; set; }
        public DateTime Ausgeliehen { get; set; }
        
        [Display(Name = "Rückgabe")]
        public DateTime Rueckgabe { get; set; }

        [Display(Name = "Zurück")]
        public bool Zurueck { get; set; }

        public virtual ApplicationUser UserProfile { get; set; }
        public virtual Kopie Kopie { get; set; }
    }
}