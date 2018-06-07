using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Helpa.Api.Models
{
    public class CustomUserLogin : IdentityUserLogin<int> { }
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    
    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {

        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {

        }
    }

    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public int? LocationId { get; set; }

        public bool RowStatus { get; set; }

        public int? GenderId { get; set; }

        public string ProfileImage { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int? AverageRating { get; set; }

        public virtual Locations Location { get; set; }

        public virtual ICollection<Notifications> SenderNotifications { get; set; }

        public virtual ICollection<Notifications> ReceiverNotifications { get; set; }

        public virtual ICollection<UserLog> UserLogs { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("HelpaContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notifications>()
                .HasRequired(m => m.Sender)
                .WithMany(t => t.SenderNotifications)
                .HasForeignKey(m => m.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notifications>()
                .HasRequired(m => m.Receiver)
                .WithMany(t => t.ReceiverNotifications)
                .HasForeignKey(m => m.ReceiverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserLog>()
                .HasRequired(m => m.User)
                .WithMany(t => t.UserLogs)
                .HasForeignKey(m => m.UserId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>>()
            //    .ToTable("Users")
            //    .HasKey(x => x.Id);

            //modelBuilder.Entity<CustomRole>()
            //    .ToTable("Roles")
            //    .HasKey(x => x.Id);

            //modelBuilder.Entity<CustomUserRole>()
            //    .ToTable("UserRoles")
            //    .HasKey(x => x.RoleId);

            //modelBuilder.Entity<CustomUserClaim>()
            //    .ToTable("UserClaims")
            //    .HasKey(x => x.Id);

            //modelBuilder.Entity<CustomUserClaim>()
            //    .ToTable("UserLogins")
            //    .HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}