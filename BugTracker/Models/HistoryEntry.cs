using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class HistoryEntry
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
    }
}