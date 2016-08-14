using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    // Comments can be added to projects or tickets, and a comment doesn't know or care about
    // the type of parent it is associated with.
    public class Comment
    {
        public int Id { get; set; }
        public int ParentId { get; set; } // Parent can be Ticket or Project

        public Comment()
        {

        }
    }
}