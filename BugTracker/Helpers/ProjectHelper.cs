using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace BugTracker.Helpers
{
    public static class ProjectHelper
    {
        /*
        private static UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public static ApplicationDbContext db { get; set; }

        private static List<ApplicationUser> AllUsers()
        {
            return manager.Users.ToList();
        }

        public static Projects FindProject(int projectId)
        {
            return db.Projects.Find(projectId);
        } 
        public static bool AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                FindProject(projectId).Users.Add(UserRolesHelper.GetUserById(userId));
                //db.SaveChanges();
            }
            return IsUserOnProject(userId, projectId);
        }

        public static bool RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                FindProject(projectId).Users.Remove(UserRolesHelper.GetUserById(userId));
                db.SaveChanges();
            }
            return !IsUserOnProject(userId, projectId);
        }

        public static bool IsUserOnProject(string userId, int projectId)
        {
            //using (ApplicationDbContext context = new ApplicationDbContext())
            //{
            //    foreach (ApplicationUser user in context.Projects.Find(projectId).Users.ToList())
            //    {
            //        if (user.Id == userId)
            //        {
            //            return true;
            //        }
            //    }
            //}

            //return false;
            return FindProject(projectId).Users.Contains(UserRolesHelper.GetUserById(userId));
        }

        public static ICollection<ApplicationUser> GetAllUsersOnProject(int projectId)
        {
            return FindProject(projectId).Users.ToList();
        }

        public static ICollection<string> GetAllUserIdsForProject(int projectId)
        {
            return FindProject(projectId).Users.Select(u => u.Id).ToList();
        }

        public static List<Projects> GetAllProjectsForUser(string userId)
        {
            List<Projects> projects = new List<Projects>();
            foreach (Projects project in db.Projects.ToList())
            {
                foreach (ApplicationUser user in project.Users)
                {
                    if (user.Id == userId)
                    {
                        projects.Add(project);
                    }
                }
            }
            return projects;
        }
        */
    }
}