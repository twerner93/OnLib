using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class Titel
    {
        public int TitelId { get; set; }
        public int AutorId { get; set; }
        public int GenreId { get; set; }
        public int TypId { get; set; }
        public string Name { get; set; }
        public string Kurzbeschreibung { get; set; }
        public string Beschreibung { get; set; }
        public string CoverPfad { get; set; }
        public DateTime Erscheinung { get; set; }
        public DateTime Created { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ApplicationUser LastModifiedBy { get; set; }
        public virtual Autor Autor { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Typ Typ { get; set; }
        //public virtual ICollection<Kopie> MyProperty { get; set; }
    }
}