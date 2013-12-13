using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace OnLib.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Email { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Autor> Autors { get; set; }
        public DbSet<Titel> Titels { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Kopie> Kopies { get; set; }
    }
}