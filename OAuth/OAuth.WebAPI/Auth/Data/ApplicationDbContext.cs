namespace OAuth.WebAPI.Auth.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OAuth.WebAPI.Auth.Models;
    using OAuth.WebAPI.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Seed();
        }

        private void Seed()
        {
            ApplicationUser existingUser = Users.FirstOrDefault(u => u.UserName.ToLower() == "johnsmith");
            if (existingUser != null)
            {
                Users.Remove(existingUser);
                SaveChanges();
            }
            ApplicationUser user = new ApplicationUser() {
                Id = Guid.NewGuid().ToString(),
                UserName = "JohnSmith",
                PasswordHash = "AEYjVGUQ1On9iz91tJIwKDpgzKsTXBKobg1oUG/aTlyMJdrHDlc2FHy9illDXTyczg==",
                Email = "JohnSmith@example.com"
            };
            Users.Add(user);
    
            user.Claims.Add(new IdentityUserClaim() { ClaimType = "Age", ClaimValue = "18", UserId = user.Id });
            user.Claims.Add(new IdentityUserClaim() { ClaimType = "Country", ClaimValue = "USA", UserId = user.Id });
            SaveChanges();

            IdentityRole roleAdmin = Roles.Add(new IdentityRole("admin"));
            IdentityRole roleManager = Roles.Add(new IdentityRole("manager"));

            user.Roles.Add(new IdentityUserRole() { RoleId = roleAdmin.Id, UserId = user.Id });
            user.Roles.Add(new IdentityUserRole() { RoleId = roleManager.Id, UserId = user.Id });

            
            SaveChanges();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}