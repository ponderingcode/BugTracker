using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            roleViewModel.SelectedUsers = UserRolesHelper.UsersInRole(applicationIdentityRole.Name).Select(u => u.Id).ToList();
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

                if (null == roleViewModel.SelectedUsers)
                {
                    UserRolesHelper.RemoveAllUsersFromRole(roleViewModel.Name);
                }
                else
                {
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
                }
                return RedirectToAction(Common.INDEX);
            }
            return View(roleViewModel);
        }
    }
}