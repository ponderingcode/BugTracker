using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    // Need to refactor (most of these properties can be inherited)
    public class Ticket
    {
        public int Id { get; set; }
        public int ProjectId { get; set; } // foreign key
        public virtual Project Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // Don't forget QA-related statuses; include entire QA lifecycle
        public string Priority { get; set; }
        public string Category { get; set; } // because "Feature Request" != "Bug"

        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateSubmitted { get; set; } // can be different from DateCreated, or null if admin or project manager creates ticket
        public DateTimeOffset? DateApproved { get; set; } // not all submitted tickets will be approved
        public DateTimeOffset? DateRejected { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateTransferred { get; set; }
        public DateTimeOffset? DateArchived { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<HistoryEntry> History { get; set; }

        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public string SubmitterId { get; set; }
        public virtual ApplicationUser Submitter { get; set; }
        public string DeveloperId { get; set; }
        public virtual ApplicationUser Developer { get; set; }
        public string TesterId { get; set; }
        public virtual ApplicationUser Tester { get; set; }
        public string AssigneeId { get; set; }
        public virtual ApplicationUser Assignee { get; set; }

        public Ticket()
        {
            this.Comments = new HashSet<Comment>();
            this.Attachments = new HashSet<Attachment>();
            this.History = new HashSet<HistoryEntry>();
        }
    }
}