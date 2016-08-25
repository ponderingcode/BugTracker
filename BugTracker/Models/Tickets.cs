using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public partial class Tickets
    {
        public Tickets()
        {
            TicketAttachments = new HashSet<TicketAttachments>();
            TicketComments = new HashSet<TicketComments>();
            Histories = new HashSet<TicketHistories>();
            Notifications = new HashSet<TicketNotifications>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Description { get; set; }
        [Required]
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        [Display(Name = "Ticket Status")]
        public int? TicketStatusId { get; set; }

        [Display(Name = "Ticket Priority")]
        public int? TicketPriorityId { get; set; }

        [Display(Name = "Ticket Type")]
        public int? TicketTypeId { get; set; }

        [Display(Name = "Project")]
        public int? ProjectId { get; set; }

        [Display(Name = "Owner")]
        public string OwnerUserId { get; set; }

        [Display(Name = "Assignee")]
        public string AssignedToUserId { get; set; }

        public bool Deleted { get; set; }

        //--- Navigation Properties  ---//

        public virtual ICollection<TicketAttachments> TicketAttachments { get; set; }

        public virtual ICollection<TicketComments> TicketComments { get; set; }

        public virtual ICollection<TicketHistories> Histories { get; set; }

        public virtual ICollection<TicketNotifications> Notifications { get; set; }

        public virtual TicketStatuses TicketStatus { get; set; }

        public virtual TicketPriorities TicketPriority { get; set; }

        public virtual TicketTypes TicketType { get; set; }

        public virtual Projects Project { get; set; }

        public virtual ApplicationUser OwnerUser { get; set; }

        public virtual ApplicationUser AssignedToUser { get; set; }
    }
}