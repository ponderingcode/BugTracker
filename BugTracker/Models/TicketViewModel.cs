using System.Collections.Generic;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SelectList Priority { get; set; }
        public SelectList Type { get; set; }
        public SelectList Status { get; set; }
        public string SelectedPriority { get; set; }
        public string SelectedType { get; set; }
        public string SelectedStatus { get; set; }
    }
}