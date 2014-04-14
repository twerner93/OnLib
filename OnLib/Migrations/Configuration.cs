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

            context.Typs.AddOrUpdate(
                t => t.Name,
                new Typ { Name = "Album"},
                new Typ { Name = "Buch"},
                new Typ { Name = "Film"},
                new Typ { Name = "Serie"}
            );

            context.Genres.AddOrUpdate(
                new Genre { Name = "TestGenre"}
            );
        }
    }
}
