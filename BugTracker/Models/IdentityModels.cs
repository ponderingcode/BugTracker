using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BugTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Attachments = new HashSet<TicketAttachments>();
            Comments = new HashSet<TicketComments>();
            Histories = new HashSet<TicketHistories>();
            Notifications = new HashSet<TicketNotifications>();
            Projects = new HashSet<Projects>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string JobTitle { get; set; }

        public virtual ICollection<Projects> Projects { get; set; }
        public virtual ICollection<TicketAttachments> Attachments { get; set; }
        public virtual ICollection<TicketComments> Comments { get; set; }
        public virtual ICollection<TicketHistories> Histories { get; set; }
        public virtual ICollection<TicketNotifications> Notifications { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Name", DisplayName));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Audit> Audit { get; set; }

        public DbSet<Tickets> Tickets { get; set; }

        public DbSet<Projects> Projects { get; set; }

        public DbSet<TicketPriorities> TicketPriorities { get; set; }

        public DbSet<TicketStatuses> TicketStatuses { get; set; }

        public DbSet<TicketTypes> TicketTypes { get; set; }

        public DbSet<TicketAttachments> TicketAttachments { get; set; }

        public DbSet<TicketComments> TicketComments { get; set; }

        public DbSet<TicketHistories> Histories { get; set; }

        public DbSet<TicketNotifications> Notifications { get; set; }
    }
}