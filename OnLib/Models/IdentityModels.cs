using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace OnLib.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Email { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Strasse { get; set; }
        public int HausNr { get; set; }
        public int PLZ { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public DateTime Geburtstag { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema:false)
        {
        }

        public DbSet<Autor> Autors { get; set; }
        public DbSet<Titel> Titels { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Typ> Typs { get; set; }
        public DbSet<Kopie> Kopies { get; set; }
        public DbSet<Leihe> Leihes { get; set; }
    }
}