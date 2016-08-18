using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public static class ProjectHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        private static Projects FindProject(int projectId)
        {
            return db.Projects.Find(projectId);
        } 

        public static bool IsUserOnProject(string userId, int projectId)
        {
            return FindProject(projectId).Users.Any(u => u.Id == userId);
        }

        public static ICollection<ApplicationUser> GetAllUsersOnProject(int projectId)
        {
            return FindProject(projectId).Users;
        }

        public static ICollection<Projects> GetAllProjectsForUser(string userId)
        {
            List<Projects> allProjects = db.Projects.ToList();
            List<Projects> allProjectsForUser = new List<Projects>();

            foreach (Projects project in allProjects)
            {
                if (IsUserOnProject(userId, project.Id))
                {
                    allProjectsForUser.Add(project);
                }
            }

            return allProjectsForUser;
        }


        public static bool AddUserToProject(string userId, int projectId)
        {
            FindProject(projectId).Users.Add(UserRolesHelper.GetUserById(userId));
            return IsUserOnProject(userId, projectId);
        }

        public static bool RemoveUserFromProject(string userId, int projectId)
        {
            FindProject(projectId).Users.Remove(UserRolesHelper.GetUserById(userId));
            return !IsUserOnProject(userId, projectId);
        }


    }
}