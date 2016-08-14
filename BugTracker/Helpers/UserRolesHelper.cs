using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public static class UserRolesHelper
    {
        private static UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private static ApplicationDbContext db = new ApplicationDbContext();

        public static bool IsUserInRole(string userId, string roleName)
        {
            return manager.IsInRole(userId, roleName);
        }

        public static ICollection<string> ListUserRoles(string userId)
        {
            return manager.GetRoles(userId);
        }

        public static bool AddUserToRole(string userId, string roleName)
        {
            var result = manager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public static bool AddUserToRoles(string userId, string[] roleNames)
        {
            var result = manager.AddToRoles(userId, roleNames);
            return result.Succeeded;
        }

        public static bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = manager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public static bool RemoveUserFromRoles(string userId, string[] roleNames)
        {
            var result = manager.RemoveFromRoles(userId, roleNames);
            return result.Succeeded;
        }

        public static ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            List<ApplicationUser> resultList = new List<ApplicationUser>();
            List<ApplicationUser> users = manager.Users.ToList();

            foreach (ApplicationUser user in users)
            {
                if (IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        public static ICollection<ApplicationUser> UsersNotInRoles(string roleName)
        {
            List<ApplicationUser> resultList = new List<ApplicationUser>();
            List<ApplicationUser> users = manager.Users.ToList();

            foreach (ApplicationUser user in users)
            {
                if (!IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        public static string GetUserIdByEmail(string userEmail)
        {
            return manager.FindByEmail(userEmail).Id;
        }
    }
}