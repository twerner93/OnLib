using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class Typ
    {
        public int TypId { get; set; }
        public string Name { get; set; }
        public string Beschreibung { get; set; }

        public virtual ICollection<Titel> Titels { get; set; }
    }
}