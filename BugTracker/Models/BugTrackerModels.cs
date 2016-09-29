using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Reflection;
using System.Linq;

namespace BugTracker.Models
{
    public static class ModelsHelper
    {
        public static List<Type> GetListOfAllModels()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => string.Equals(t.Namespace, "BugTracker.Models", StringComparison.Ordinal)).ToList();
        }
    }

    /*
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

        //private string title;
        //[Required]
        //public string Title
        //{
        //    get
        //    {
        //        return title;
        //    }
        //    set
        //    {
        //        if (value != title)
        //        {
        //            title = value;
        //            NotifyPropertyChanged(Common.TITLE);
        //        }
        //    }
        //}

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

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    //Raise PropertyChanged event
        //    //PropertyChanged.BeginInvoke(this, )
        //}
    }
    */

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

        public System.DateTimeOffset Created { get; set; }
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
        [Display(Name = "Old Value")]
        public string OldValue { get; set; }
        [Display(Name = "New Value")]
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

        [Required]
        public string Name { get; set; }

        [Display(Name = "Project Manager")]
        public string ProjectManagerUserId { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM.dd.yyyy}")]
        public DateTimeOffset? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM.dd.yyyy}")]
        public DateTimeOffset? EndDate { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }

}