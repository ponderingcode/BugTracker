using System.Web.Mvc;
using System.Collections.Generic;

namespace BugTracker.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public MultiSelectList Users { get; set; }
        public List<string> SelectedUsers { get; set; }
    }
}