using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public partial class Tickets
    {
        public Tickets()
        {
            TicketAttachments = new HashSet<TicketAttachments>();
            TicketComments = new HashSet<TicketComments>();
            Histories = new HashSet<TicketHistories>();
            Notifications = new HashSet<TicketNotifications>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Description { get; set; }
        [Required]
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        [Display(Name = "Ticket Status")]
        public int? TicketStatusId { get; set; }

        [Display(Name = "Ticket Priority")]
        public int? TicketPriorityId { get; set; }

        [Display(Name = "Ticket Type")]
        public int? TicketTypeId { get; set; }

        [Display(Name = "Project")]
        public int? ProjectId { get; set; }

        [Display(Name = "Owner")]
        public string OwnerUserId { get; set; }

        [Display(Name = "Assignee")]
        public string AssignedToUserId { get; set; }

        public bool Deleted { get; set; }

        //--- Navigation Properties  ---//

        public virtual ICollection<TicketAttachments> TicketAttachments { get; set; }

        public virtual ICollection<TicketComments> TicketComments { get; set; }

        public virtual ICollection<TicketHistories> Histories { get; set; }

        public virtual ICollection<TicketNotifications> Notifications { get; set; }

        public virtual TicketStatuses TicketStatus { get; set; }

        public virtual TicketPriorities TicketPriority { get; set; }

        public virtual TicketTypes TicketType { get; set; }

        public virtual Projects Project { get; set; }

        public virtual ApplicationUser OwnerUser { get; set; }

        public virtual ApplicationUser AssignedToUser { get; set; }

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

        public Tickets GetData(int id)
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
            Tickets originalItem = db.Tickets.FirstOrDefault(t => t.Id ==  updatedItem.Id);
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