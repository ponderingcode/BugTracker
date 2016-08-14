namespace BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
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

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == Role.ADMINISTRATOR))
            {
                roleManager.Create(new IdentityRole { Name = Role.ADMINISTRATOR });
            }

            if (!context.Roles.Any(r => r.Name == Role.DEVELOPER))
            {
                roleManager.Create(new IdentityRole { Name = Role.DEVELOPER });
            }

            if (!context.Roles.Any(r => r.Name == Role.PROJECT_MANAGER))
            {
                roleManager.Create(new IdentityRole { Name = Role.PROJECT_MANAGER });
            }

            if (!context.Roles.Any(r => r.Name == Role.SUBMITTER))
            {
                roleManager.Create(new IdentityRole { Name = Role.SUBMITTER });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "ryan@ponderingcode.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ryan@ponderingcode.com",
                    Email = "ryan@ponderingcode.com",
                    FirstName = "Ryan",
                    LastName = "Fleming",
                    DisplayName = "Ryan Fleming",
                }, "CoderFoundry");
            }

            var administratorUserID = userManager.FindByEmail("ryan@ponderingcode.com").Id;
            userManager.AddToRole(administratorUserID, Role.ADMINISTRATOR);
        }
    }
}
