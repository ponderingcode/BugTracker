using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ValueObjects
{
    public class UserVO
    {
        public string FirstName       { get; private set; }
        public string LastName        { get; private set; }
        public string DisplayName     { get; private set; }
        public string Email           { get; private set; }
        public string UserName        { get; private set; }
        public ICollection<string> UserRoles { get; private set; }

        public UserVO(AspNetUser user, ICollection<string> userRoles)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            DisplayName = user.DisplayName;
            Email = user.Email;
            UserName = user.UserName;
            UserRoles = userRoles;
        }
    }
}