using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public static class ProjectHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private static List<ApplicationUser> AllUsers()
        {
            return manager.Users.ToList();
        }

        public static Projects FindProject(int projectId)
        {
            return db.Projects.Find(projectId);
        } 

        public static bool IsUserOnProject(string userId, int projectId)
        {
            return FindProject(projectId).Users.Any(u => u.Id == userId);
        }

        public static ICollection<ApplicationUser> GetAllUsersOnProject(int projectId)
        {
            List<string> userIdList = db.ProjectUsers.Where(p => p.ProjectId == projectId).Select(u => u.UserId).ToList();
            List<ApplicationUser> resultList = new List<ApplicationUser>();
            foreach (string userId in userIdList)
            {
                resultList.Add(UserRolesHelper.GetUserById(userId));
            }
            return resultList;
        }

        public static string[] GetAllUserIdsForProject(int projectId)
        {
            return db.ProjectUsers.Where(p => p.ProjectId == projectId).Select(u => u.UserId).ToArray();
        }

        //public static ICollection<Projects> GetAllProjectsForUser(string userId)
        //{
        //    List<Projects> allProjects = db.Projects.ToList();
        //    List<Projects> allProjectsForUser = new List<Projects>();

        //    foreach (Projects project in allProjects)
        //    {
        //        if (IsUserOnProject(userId, project.Id))
        //        {
        //            allProjectsForUser.Add(project);
        //        }
        //    }

        //    return allProjectsForUser;
        //}

        public static List<Projects> GetAllProjectsForUser(string userId)
        {
            List<int> projectIdList = db.ProjectUsers.Where(p => p.UserId == userId).Select(p => p.ProjectId).ToList();
            List<Projects> projectsList = new List<Projects>();
            if (0 < projectIdList.Count)
            {
                foreach (int projectId in projectIdList)
                {
                    projectsList.Add((Projects)db.Projects.Where(p => p.Id == projectId));
                }
            }

            return projectsList;
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