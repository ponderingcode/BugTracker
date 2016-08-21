using BugTracker.Authorization;
using BugTracker.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [VerboseAuthorize(Roles = Role.STAKEHOLDERS)]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        [Authorize(Roles = Role.ADMINISTRATOR + "," + Role.PROJECT_MANAGER)]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }

            return View(projects);
        }

        // GET: Projects/Create
        [Authorize(Roles = Role.ADMINISTRATOR)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartDate,EndDate")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = Role.ADMINISTRATOR + "," + Role.PROJECT_MANAGER)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projectData = db.Projects.Find(id);
            if (projectData == null)
            {
                return HttpNotFound();
            }
            ProjectViewModel projectViewModel = new ProjectViewModel { Id = projectData.Id, Name = projectData.Name };
            projectViewModel.Users = new MultiSelectList(db.Users, Common.ID, Common.USER_NAME);
            projectViewModel.SelectedUsers = GetAllUserIdsForProject(projectData.Id).ToList();
            return View(projectViewModel);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
            {
                Projects projectData = db.Projects.Find(projectViewModel.Id);
                var usersOnProject = GetAllUsersOnProject(projectData.Id).Select(u => u.Id);

                if (null == projectViewModel.SelectedUsers)
                {
                    RemoveAllUsersFromProject(projectData.Id);
                }
                else
                {
                    foreach (var userId in usersOnProject)
                    {
                        if (!projectViewModel.SelectedUsers.Contains(userId))
                        {
                            RemoveUserFromProject(userId, projectData.Id);
                        }
                    }
                    foreach (var userId in projectViewModel.SelectedUsers)
                    {
                        if (!IsUserOnProject(userId, projectData.Id))
                        {
                            AddUserToProject(userId, projectData.Id);
                        }
                    }
                }
                // navigate back to the roles index page
                return RedirectToAction(Common.INDEX);
            }
            return View(projectViewModel);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = Role.ADMINISTRATOR)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //
        // Helper functions
        //

        public Projects FindProject(int projectId)
        {
            return db.Projects.Find(projectId);
        }

        public bool IsUserOnProject(string userId, int projectId)
        {
            List<ApplicationUser> projectUsers = db.Projects.Find(projectId).Users.ToList();
            foreach (ApplicationUser user in projectUsers)
            {
                if (user.Id == userId)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                db.Projects.Find(projectId).Users.Add(db.Users.Find(userId));
                db.SaveChanges();
            }
            return IsUserOnProject(userId, projectId);
        }

        public bool RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                db.Projects.Find(projectId).Users.Remove(db.Users.Find(userId));
                db.SaveChanges();
            }
            return !IsUserOnProject(userId, projectId);
        }

        public bool RemoveAllUsersFromProject(int projectId)
        {
            db.Projects.Find(projectId).Users.Clear();
            db.SaveChanges();
            return 0 == db.Projects.Find(projectId).Users.Count;
        }

        public ICollection<ApplicationUser> GetAllUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users.ToList();
        }

        public ICollection<string> GetAllUserIdsForProject(int projectId)
        {
            return db.Projects.Find(projectId).Users.Select(u => u.Id).ToList();
        }

        public List<Projects> GetAllProjectsForUser(string userId)
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
    }
}