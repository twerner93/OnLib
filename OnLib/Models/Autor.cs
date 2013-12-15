using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class Autor
    {
        public int AutorId { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public string Sonstiges { get; set; }

        public virtual ICollection<Titel> Titels { get; set; }
    }
}