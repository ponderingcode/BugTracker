using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    //
    // WISH LIST
    //
    // 1. Project is currently the top-tier unit of organization; add higher tiers for companies & divisions.
    //    Doing so will be useful for everything from small conglomerates to international businesses with continent or country-based divisions.
    //    These additional tiers will necessitate the creation of additional roles & modification of existing roles, such as
    //    "CompanyAdministrator", "DivisionAdministrator", & "SuperAdministrator"
    //
    // 2. Add comprehensive audit trail/chain of custody to this, tickets, users, etc.
    //
    public class Project
    {
        public int Id { get; set; }
        //public int? CompanyId { get; set; }
        //public int? DivisionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; } // Projects have priorities just like tickets do, especially when a project is the CEO's idea

        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateTransferred { get; set; } // Timestamp of when project manager was assigned to project by admin
        public DateTimeOffset? DateArchived { get; set; } // Allow project manager to archive/hide old projects

        public virtual ICollection<Ticket> Tickets { get; set; }
        //public virtual ICollection<Comment> Comments { get; set; } // Projects should support commenting, too.
        //public virtual ICollection<Attachment> Attachments { get; set; } // Project-level attachment(s)
        //public virtual ICollection<HistoryEntry> History { get; set; } // Project-level audit trail

        public string CreatorId { get; set; } // always an admin
        public virtual ApplicationUser Creator { get; set; }
        public string AdministratorId { get; set; } // can be different than creator - admin who created project could quit, be fired, etc.
        public virtual ApplicationUser Administrator { get; set; }
        public string ProjectManagerId { get; set; }
        public virtual ApplicationUser ProjectManager { get; set; }
        public virtual ICollection<ApplicationUser> Developers { get; set; }
        //public virtual ICollection<ApplicationUser> Testers { get; set; } // QA (no QA role exists yet)

        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            //this.Comments = new HashSet<Comment>();
            //this.Attachments = new HashSet<Attachment>();
            //this.History = new HashSet<HistoryEntry>();
            this.Developers = new HashSet<ApplicationUser>();
        }
    }
}