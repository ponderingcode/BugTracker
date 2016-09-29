using BugTracker.Authorization;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<Tickets> tickets = new List<Tickets>();
            List<Projects> projects = new List<Projects>();
            ICollection<string> userRoles = UserRolesHelper.ListUserRoles(userId);

            if (userRoles.Contains(Role.ADMINISTRATOR))
            {
                // All tickets
                tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType).ToList();
            }
            else if (userRoles.Contains(Role.SUBMITTER))
            {
                // Tickets owned by Submitter
                tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
            }
            else if (userRoles.Contains(Role.DEVELOPER))
            {
                tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
            }
            else if (userRoles.Contains(Role.PROJECT_MANAGER))
            {
                //projects = db.Projects.Where(p => p.ProjectManagerUserId == userId).ToList();
                //foreach (Projects project in projects)
                //{
                //    tickets.Concat(project.Tickets);
                //}

                ApplicationUser user = UserRolesHelper.GetUserById(userId);
                foreach (Projects project in db.Projects.ToList())
                {
                    foreach (ApplicationUser projectUser in project.Users)
                    {
                        if (userId == projectUser.Id)
                        {
                            tickets.AddRange(project.Tickets);
                        }
                    }
                }

            }
            return View(tickets);
        }

        [VerboseAuthorize(Roles = Role.DEVELOPER)]
        public ActionResult TicketsAssignedToDeveloper()
        {
            string userId = User.Identity.GetUserId();
            List<Tickets> tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
            ViewBag.Tickets = tickets;
            return View();
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAuthenticated)
            {
                ViewBag.CurrentUser = User.Identity.GetUserId();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        [VerboseAuthorize(Roles = Role.SUBMITTER)]
        public ActionResult Create()
        {
            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.OwnerUserId = User.Identity.Name;
            ViewBag.Created = DateTimeOffset.Now;
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            //ViewBag.TicketPriorityId = new SelectList(PriorityName.AsDataProvider);
            //ViewBag.TicketStatusId = StatusName.SUBMITTED;
            //ViewBag.TicketTypeId = new SelectList(TicketTypeName.AsDataProvider);
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,TicketStatusId,TicketPriorityId,TicketTypeId,ProjectId,OwnerUserId,AssignedToUserId")] Tickets ticket, HttpPostedFileBase image)
        {
            if (null != image && 0 < image.ContentLength)
            {
                string ext = Path.GetExtension(image.FileName).ToLower();
                if (FileType.BMP != ext &&
                    FileType.GIF != ext &&
                    FileType.JPG != ext &&
                    FileType.JPEG != ext &&
                    FileType.PNG != ext)
                {
                    ModelState.AddModelError("image", "Invalid Format.");
                }
            }

            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;

                if (null != image)
                {
                    TicketAttachments ticketAttachment = new TicketAttachments();
                    string filePath = "/Uploads/"; // relative server path
                    string absPath = Server.MapPath("~" + filePath); // path on physical drive on server
                    ticketAttachment.FileURL = filePath + image.FileName; // media URL for relative path
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                    ticket.TicketAttachments.Add(ticketAttachment);
                }

                ticket.OwnerUserId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }

           
            ViewBag.Created = tickets.Created;
            //ViewBag.Updated = DateTimeOffset.Now;
            tickets.Updated = DateTimeOffset.Now;
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,TicketStatusId,TicketPriorityId,TicketTypeId,ProjectId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                Tickets original = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == tickets.Id);
                List<PropertyInfo> propertyInfoList = tickets.GetType().GetProperties().ToList();

                foreach (PropertyInfo propertyInfo in propertyInfoList)
                {
                    var oldValue = original.GetType().GetProperty(propertyInfo.Name)?.GetValue(original, null);
                    var newValue = tickets.GetType().GetProperty(propertyInfo.Name)?.GetValue(tickets, null);
                    string strOldValue = oldValue?.ToString() ?? string.Empty;
                    string strNewValue = newValue?.ToString() ?? string.Empty;

                    if (!strOldValue.Equals(strNewValue) && !strNewValue.Equals(string.Empty))
                    {
                        TicketHistories historyEntry = new TicketHistories()
                        {
                            TicketId = tickets.Id,
                            Property = propertyInfo.Name,
                            OldValue = strOldValue,
                            NewValue = strNewValue,
                            UserId = tickets.AssignedToUserId
                        };
                        db.Histories.Add(historyEntry);

                        if (propertyInfo.Name.Equals("AssignedToUserId"))
                        {
                            CreateNotification(historyEntry);
                        }
                    }
                }
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            tickets.Updated = DateTimeOffset.Now;
            return View(tickets);
        }

        // GET: Tickets/Archive/5
        [VerboseAuthorize(Roles = Role.ADMINISTRATOR)]
        public ActionResult Archive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        [VerboseAuthorize(Roles = Role.ADMINISTRATOR)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Need to check for attachments and delete those prior to deleting a ticket
            Tickets tickets = db.Tickets.Find(id);
            if (0 < tickets.TicketAttachments.Count)
            {
                tickets.TicketAttachments.Clear();
            }
            db.Tickets.Remove(tickets);
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

        private void CreateNotification(TicketHistories historyEntry)
        {
            db.Notifications.Add
            (
                new TicketNotifications
                {
                    TicketId = historyEntry.TicketId,
                    UserId = historyEntry.UserId,
                    HasBeenRead = false
                }
            );

            string ticketUrl = Url.Action(ActionName.DETAILS, ControllerName.TICKETS, new { id = historyEntry.TicketId }, protocol: Request.Url.Scheme);
            ApplicationUser developer = db.Users.Find(historyEntry.UserId);
            IdentityMessage notificationMessage = new IdentityMessage
            {
                Destination = developer.Email,
                Subject = "A ticket has been assigned to you.",
                Body = "<a href=" + ticketUrl + ">Take a look</a>"
            };
            EmailService emailService = new EmailService();
            emailService.SendAsync(notificationMessage);
        }
    }
}
