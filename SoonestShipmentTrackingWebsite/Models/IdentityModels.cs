using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }

        // Backs the code-input email verification flow (Account/VerifyEmail.aspx).
        // IdentityUser.EmailConfirmed is the actual "is verified" flag; these
        // two just hold the pending one-time code and its expiry.
        public string EmailVerificationCode { get; set; }
        public DateTime? EmailVerificationCodeExpiresAt { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("database", throwIfV1Schema: false)
        {
        }

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentHistory> ShipmentHistories { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shipment>()
                .HasMany(s => s.History)
                .WithRequired(h => h.Shipment)
                .HasForeignKey(h => h.ShipmentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Shipment>()
                .Property(s => s.ControlNumber)
                .IsRequired();
        }
    }
}