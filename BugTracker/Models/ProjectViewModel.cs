using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MultiSelectList AllUsers { get; set; }
        [Display(Name = "Assigned Users")]
        public string[] AssignedUsers { get; set; }
    }
}