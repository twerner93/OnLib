namespace OnLib.Migrations
{
    using OnLib.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OnLib.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(OnLib.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Autors.AddOrUpdate(
                a => a.Nachname,
                new Autor { Nachname = "Green Day" },
                new Autor { Nachname = "Linkin Park"}
            );

            context.Genres.AddOrUpdate(
                g => g.Name,
                new Genre { Name = "Rock" },
                new Genre { Name = "Punk-Rock" },
                new Genre { Name = "Pop" }
            );

        }
    }
}
