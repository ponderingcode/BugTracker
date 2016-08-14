using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //public bool IsUSerOnProject(string userId, int projectId)
        //{
        //    var project = db.Projects.Find(projectId);
        //    var flag = project.Users.Any(u => u.Id == userId);
        //    return flag;
        //}
    }
}