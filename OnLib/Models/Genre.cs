using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnLib.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Titel> Titels  { get; set; }
    }
}