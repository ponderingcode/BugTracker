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
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: UserRoles
        public ActionResult Index()
        {
            List<IdentityRole> rolesList = db.Roles.ToList();
            List<RoleViewModel> roles = new List<RoleViewModel>();
            roles = rolesList.Select(u => new RoleViewModel { Id = u.Id, Name = u.Name }).ToList();
            return View(roles);
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
            RoleViewModel roleViewModel = new RoleViewModel { Id = applicationIdentityRole.Id, Name = applicationIdentityRole.Name };
            roleViewModel.Users = new MultiSelectList(db.Users, Common.ID, Common.USER_NAME);
            roleViewModel.SelectedUsers = UserRolesHelper.UsersInRole(applicationIdentityRole.Name).Select(u => u.Id).ToArray();
            return View(roleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = db.Roles.Find(roleViewModel.Id);
                var usersInRole = UserRolesHelper.UsersInRole(roleViewModel.Name).Select(u => u.Id);

                foreach (var userId in usersInRole)
                {
                    if (null != roleViewModel.SelectedUsers && !roleViewModel.SelectedUsers.Contains(userId))
                    {
                        await UserRolesHelper.RemoveUserFromRoleAsync(userId, roleViewModel.Name);
                    }
                }

                foreach (var userId in roleViewModel.SelectedUsers)
                {
                    if (!UserRolesHelper.IsUserInRole(userId, roleViewModel.Name))
                    {
                        await UserRolesHelper.AddUserToRoleAsync(userId, roleViewModel.Name);
                    }
                }

                // navigate back to the roles index page
                return RedirectToAction(Common.INDEX);
            }
            return View(roleViewModel);
        }
    }
}