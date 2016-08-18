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

        public int? TicketStatusId { get; set; }

        public int? TicketPriorityId { get; set; }

        public int? TicketTypeId { get; set; }

        public int? ProjectId { get; set; }

        public string OwnerUserId { get; set; }

        public string AssignedToUserId { get; set; }

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


    public partial class TicketAttachments
    {
        public int Id { get; set; }
        public int? TicketId { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserId { get; set; }
        public string FileURL { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }


    public partial class TicketComments
    {
        public int Id { get; set; }
        [AllowHtml]
        [Required]
        public string Comment { get; set; }

        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }


    public partial class NotificationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class TicketHistories
    {
        public int Id { get; set; }
        public int? TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public bool? Changed { get; set; }
        public string UserId { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }


    public partial class TicketNotifications
    {
        public int Id { get; set; }
        public int? TicketId { get; set; }
        public string UserId { get; set; }
        public bool HasBeenRead { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }



    public partial class TicketPriorities
    {
        public TicketPriorities()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }


    public partial class TicketStatuses
    {
        public TicketStatuses()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }


    public partial class TicketTypes
    {
        public TicketTypes()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }


    public partial class Projects
    {
        public Projects()
        {
            Users = new HashSet<ApplicationUser>();
            Tickets = new HashSet<Tickets>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM.dd.yyyy}")]
        public DateTimeOffset? StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM.dd.yyyy}")]
        public DateTimeOffset? EndDate { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }

    public partial class ProjectUsers
    {
        [Key]
        public int ProjectId { get; set; }
        public string UserId { get; set; }
    }

}