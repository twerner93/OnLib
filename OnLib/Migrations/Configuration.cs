namespace OnLib.Migrations
{
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
                new Models.Typ { Name = "Album" },
                new Models.Typ { Name = "Buch" },
                new Models.Typ { Name = "Film" },
                new Models.Typ { Name = "Serie" }
            );

            context.Genres.AddOrUpdate(
                g => g.Name,
                new Models.Genre { Name = "Rock", Typ = "Album" },
                new Models.Genre { Name = "Test", Typ = "Album" },
                new Models.Genre { Name = "Test", Typ = "Buch" },
                new Models.Genre { Name = "Test", Typ = "Film" },
                new Models.Genre { Name = "Test", Typ = "Serie" }
            );
        }
    }
}
