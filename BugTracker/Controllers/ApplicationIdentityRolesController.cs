using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using BugTracker.Helpers;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    [Authorize(Roles = Role.ADMINISTRATOR)]
    public class ApplicationIdentityRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: UserRoles
        public ActionResult Index()
        {
            List<IdentityRole> applicationIdentityRoles = db.Roles.ToList();
            List<ApplicationIdentityRolesViewModel> applicationIdentityRolesViewModel = new List<ApplicationIdentityRolesViewModel>();
            applicationIdentityRolesViewModel = applicationIdentityRoles.Select(u => new ApplicationIdentityRolesViewModel { ApplicationIdentityRoleId = u.Id, ApplicationIdentityRoleName = u.Name }).ToList();
            return View(applicationIdentityRolesViewModel);
        }

        public ActionResult SingleRoleIndex(string roleName)
        {
            ICollection<ApplicationUser> usersInRole = UserRolesHelper.UsersInRole(roleName);
            ViewBag.usersInRole = usersInRole;
            ViewBag.banner = "Assign/Unassign " + roleName + "s";
            ViewBag.roleName = roleName + "s";
            return View();
        }

        public ActionResult Edit(string applicationIdentityRoleId)
        {
            if (null == applicationIdentityRoleId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole applicationIdentityRole = db.Roles.Find(applicationIdentityRoleId);
            if (null == applicationIdentityRole)
            {
                return HttpNotFound();
            }
            ApplicationIdentityRolesViewModel applicationIdentityRolesViewModel = new ApplicationIdentityRolesViewModel { ApplicationIdentityRoleId = applicationIdentityRole.Id, ApplicationIdentityRoleName = applicationIdentityRole.Name };
            applicationIdentityRolesViewModel.ApplicationIdentityRoleUsers = new MultiSelectList(db.Users, Common.ID, Common.USER_NAME);
            applicationIdentityRolesViewModel.SelectedApplicationIdentityRoleUsers = UserRolesHelper.UsersInRole(applicationIdentityRole.Name).Select(u => u.Id).ToArray();
            return View(applicationIdentityRolesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationIdentityRolesViewModel applicationIdentityRolesViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole applicationIdentityRole = db.Roles.Find(applicationIdentityRolesViewModel.ApplicationIdentityRoleId);
                var applicationIdentityRoleUsers = UserRolesHelper.UsersInRole(applicationIdentityRolesViewModel.ApplicationIdentityRoleName).Select(u => u.Id);

                foreach (var userId in applicationIdentityRoleUsers)
                {
                    if (!applicationIdentityRolesViewModel.SelectedApplicationIdentityRoleUsers.Contains(userId))
                    {
                        await UserRolesHelper.RemoveUserFromRoleAsync(userId, applicationIdentityRolesViewModel.ApplicationIdentityRoleName);
                    }
                }

                foreach (var userId in applicationIdentityRolesViewModel.SelectedApplicationIdentityRoleUsers)
                {
                    if (!UserRolesHelper.IsUserInRole(userId, applicationIdentityRolesViewModel.ApplicationIdentityRoleName))
                    {
                        await UserRolesHelper.AddUserToRoleAsync(userId, applicationIdentityRolesViewModel.ApplicationIdentityRoleName);
                    }
                }

                // navigate back to the roles index page
                return RedirectToAction(Common.INDEX);
            }
            return View(applicationIdentityRolesViewModel);
        }
    }
}