using System;
using System.Collections.Generic;

namespace BugTracker.Models
{
    public partial class Audit
    {
        public int Id { get; set; }
        public int KeyFieldId { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string DataModel { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
        public string Changes { get; set; }
        public int AuditActionTypeEnum { get; set; }
    }

    public class AuditChange
    {
        public string DateTimeStamp { get; set; }
        public AuditActionType AuditActionType { get; set; }
        public string AuditActionTypeName { get; set; }
        public List<AuditDelta> Changes { get; set; }
        public AuditChange()
        {
            Changes = new List<AuditDelta>();
        }
    }

    public class AuditDelta
    {
        public string FieldName { get; set; }
        public string ValueBefore { get; set; }
        public string ValueAfter { get; set; }
    }

    public enum AuditActionType
    {
        Create = 1,
        Update,
        Delete
    }
}