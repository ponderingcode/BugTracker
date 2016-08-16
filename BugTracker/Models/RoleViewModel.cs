using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; } // select control needs array of strings
    }
}