using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Authorization;
using Microsoft.AspNet.Identity;
using BugTracker.Helpers;
using System.IO;
using Newtonsoft.Json;
using KellermanSoftware.CompareNetObjects;

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

            if (UserRolesHelper.IsUserInRole(userId, Role.ADMINISTRATOR))
            {
                // All tickets
                tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType).ToList();
            }
            else if (UserRolesHelper.IsUserInRole(userId, Role.SUBMITTER))
            {
                // Tickets owned by Submitter
                tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
            }
            else
            {
                // ***** NEEDS REFACTORING (LINQ will save the day)
                //
                // Project Manager Index & Standard Index for Developer
                //
                
                // Tickets for projects to which the Project Manager or Developer is assigned

                // Create a list of all projects the user is involved with
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
                // potentially long list, so we only iterate through it once
                foreach (Tickets ticket in db.Tickets.ToList())
                {
                    // much smaller list than tickets
                    foreach (Projects project in projects)
                    {
                        if (ticket.Project.Id == project.Id)
                        {
                            tickets.Add(ticket);
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
            ViewBag.Updated = DateTimeOffset.Now;
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

        // AUDIT TRAIL
        public void CreateAuditTrail(AuditActionType actionType, int keyFieldId, object unchangedItem, object changedItem)
        {
            // get the differance
            CompareLogic comparison = new CompareLogic();
            comparison.Config.MaxDifferences = 99;
            ComparisonResult comparisonResult = comparison.Compare(unchangedItem, changedItem);
            List<AuditDelta> deltaList = new List<AuditDelta>();
            foreach (Difference change in comparisonResult.Differences)
            {
                AuditDelta delta = new AuditDelta();

                if ("." == change.PropertyName.Substring(0, 1))
                {
                    delta.FieldName = change.PropertyName.Substring(1, change.PropertyName.Length - 1);
                }
                delta.ValueBefore = change.Object1Value;
                delta.ValueAfter = change.Object2Value;
                deltaList.Add(delta);
            }
            Audit audit = new Audit();
            audit.AuditActionTypeEnum = (int)actionType;
            audit.DataModel = GetType().Name;
            audit.DateTimeStamp = DateTime.Now;
            audit.KeyFieldId = keyFieldId;
            audit.ValueBefore = JsonConvert.SerializeObject(unchangedItem); // if use xml instead of json, can use xml annotation to describe field names etc better
            audit.ValueAfter = JsonConvert.SerializeObject(changedItem);
            audit.Changes = JsonConvert.SerializeObject(deltaList);
            ApplicationDbContext db = new ApplicationDbContext();
            db.Audit.Add(audit);
            db.SaveChanges();
        }

        public Tickets ReadRecord(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Tickets record = db.Tickets.FirstOrDefault(t => t.Id == id);
            //Tickets model = record ?? new Tickets();
            //return model;
            return record ?? new Tickets();
        }

        public List<Tickets> GetAllData(bool showDeleted)
        {
            List<Tickets> resultList = new List<Tickets>();
            ApplicationDbContext db = new ApplicationDbContext();
            List<Tickets> searchResults = new List<Tickets>();

            searchResults = showDeleted ? db.Tickets.ToList() : db.Tickets.Where(t => t.Deleted == false).ToList();

            foreach (Tickets searchResult in searchResults)
            {
                Tickets record = new Tickets();
                record = searchResult;
                resultList.Add(record);
            }
            return resultList;
        }

        public void CreateRecord(Tickets original)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Tickets record = new Tickets();
            record = original;
            db.Tickets.Add(record);
            db.SaveChanges(); // save first so we get back the record's Id for audit tracking
            Tickets dummyObj = new Tickets(); // Storage of this null object shows data before creation = nix, naught, nothing!
            CreateAuditTrail(AuditActionType.Create, record.Id, dummyObj, record);
        }

        public bool UpdateRecord(Tickets updatedItem)
        {
            bool success = false;
            ApplicationDbContext db = new ApplicationDbContext();
            Tickets originalItem = db.Tickets.FirstOrDefault(t => t.Id == updatedItem.Id);
            if (null != originalItem)
            {
                // copy the original values
                Tickets copiedItem = new Tickets();
                copiedItem = originalItem;
                // update the original
                originalItem = updatedItem;
                db.SaveChanges();

                CreateAuditTrail(AuditActionType.Update, updatedItem.Id, copiedItem, updatedItem);
                success = true;
            }
            return success;
        }

        public void DeleteRecord(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Tickets record = db.Tickets.FirstOrDefault(t => t.Id == id);
            if (null != record)
            {
                Tickets dummyObj = new Tickets(); // Storage of this null object shows data after delete = nix, naught, nothing!
                record.Deleted = true;
                db.SaveChanges();
                CreateAuditTrail(AuditActionType.Delete, id, record, dummyObj);
            }
        }
    }
}
