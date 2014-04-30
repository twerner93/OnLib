using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class TitelViewModel
    {
        public int TitelId { get; set; }
        public int GenreId { get; set; }
        public int TypId { get; set; }
        
        [Required]
        public string AutorNachname { get; set; }
        public string AutorVorname { get; set; }
        public string Name { get; set; }
        public string Kurzbeschreibung { get; set; }
        public string Beschreibung { get; set; }
        public DateTime Erscheinung { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public virtual ApplicationUser LastModifiedBy { get; set; }
        public virtual Autor Autor { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Typ Typ { get; set; }
        public virtual ICollection<Kopie> Kopies { get; set; }
    }
}