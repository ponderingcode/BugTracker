using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ApplicationIdentityRolesViewModel
    {
        public string ApplicationIdentityRoleId { get; set; }
        public string ApplicationIdentityRoleName { get; set; }
        public MultiSelectList ApplicationIdentityRoleUsers { get; set; }
        public string[] SelectedApplicationIdentityRoleUsers { get; set; } // select control needs array of strings
    }
}