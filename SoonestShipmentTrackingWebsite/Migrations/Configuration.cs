using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));

            if (!roleManager.RoleExists("Customer"))
                roleManager.Create(new IdentityRole("Customer"));

            const string adminEmail = "admin@soonestglobalexpress.local";
            const string adminPassword = "Admin@12345";

            if (userManager.FindByEmail(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "System Administrator",
                    PhoneNumber = "0000000000"
                };

                var result = userManager.Create(admin, adminPassword);
                if (result.Succeeded)
                {
                    userManager.AddToRole(admin.Id, "Admin");
                }
            }
        }
    }
}