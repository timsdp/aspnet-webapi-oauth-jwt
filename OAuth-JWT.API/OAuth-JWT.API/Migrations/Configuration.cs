namespace OAuth_JWT.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OAuth_JWT.API.Infrastructure;
    using OAuth_JWT.API.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<OAuth_JWT.API.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "SuperAdmin",
                Email = "superadmin@example.com",
                EmailConfirmed = true,
                FirstName = "John",
                LastName = "Smith",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

        }
    }
}
