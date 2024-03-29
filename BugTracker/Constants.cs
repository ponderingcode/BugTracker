﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugTracker
{
    public static class Role
    {
        public const string ADMINISTRATOR   = "Administrator";
        public const string PROJECT_MANAGER = "ProjectManager";
        public const string DEVELOPER       = "Developer";
        public const string SUBMITTER       = "Submitter";
        
        /// <summary>
        /// All roles except for Submitter
        /// </summary>
        public const string STAKEHOLDERS = ADMINISTRATOR + "," + PROJECT_MANAGER + "," + DEVELOPER;
    }

    public static class PriorityName
    {
        public const string LOW    = "Low";
        public const string MEDIUM = "Medium";
        public const string HIGH   = "High";

        public static List<string> AsDataProvider = new List<string>()
        {
            LOW,
            MEDIUM,
            HIGH
        };
    }

    public static class StatusName
    {
        public const string SUBMITTED        = "Submitted";
        public const string IN_QA            = "In QA";
        public const string IN_DEVELOPMENT   = "In Development";
        public const string PENDING_APPROVAL = "Pending Approval";
        public const string APPROVED         = "Approved";
        public const string REJECTED         = "Rejected";
        public const string UNASSIGNED       = "Unassigned";
        public const string CLOSED           = "Closed";

        public static List<string> AsDataProvider = new List<string>()
        {
            SUBMITTED,
            PENDING_APPROVAL,
            APPROVED,
            REJECTED,
            IN_DEVELOPMENT,
            IN_QA,
            CLOSED
        };
    }

    public static class TicketTypeName
    {
        public const string BUG             = "Bug";
        public const string FEATURE_REQUEST = "Feature Request";

        public static List<string> AsDataProvider = new List<string>()
        {
            BUG,
            FEATURE_REQUEST
        };
    }

    public static class Common
    {
        public const string ACTIVE              = "active";
        public const string ID                  = "Id";
        public const string TITLE               = "Title";
        public const string DESCRIPTION         = "Description";
        public const string UPDATED             = "Updated";
        public const string USER_NAME           = "UserName";
        public const string MULTIPART_FORM_DATA = "Multipart/form-data";
    }

    public static class ActionName
    {
        public const string ARCHIVE                       = "Archive";
        public const string ATTACHMENT_DETAILS            = "AttachmentDetails";
        public const string CHANGE_PROFILE_INFO           = "ChangeProfileInfo";
        public const string CHANGE_PASSWORD               = "ChangePassword";
        public const string CREATE                        = "Create";
        public const string DELETE                        = "Delete";
        public const string DELETE_ATTACHMENT             = "DeleteAttachment";
        public const string DETAILS                       = "Details";
        public const string EDIT                          = "Edit";
        public const string INDEX                         = "Index";
        public const string LOGIN                         = "Login";
        public const string LOGIN_AS_DEMO_ADMINISTRATOR   = "LoginAsDemoAdministrator";
        public const string LOGIN_AS_DEMO_DEVELOPER       = "LoginAsDemoDeveloper";
        public const string LOGIN_AS_DEMO_PROJECT_MANAGER = "LoginAsDemoProjectManager";
        public const string LOGIN_AS_DEMO_SUBMITTER       = "LoginAsDemoSubmitter";
        public const string LOG_OFF                       = "LogOff";
        public const string SET_PASSWORD                  = "SetPassword";
    }

    public static class ControllerName
    {
        public const string ACCOUNT         = "Account";
        public const string HOME            = "Home";
        public const string MANAGE          = "Manage";
        public const string PROJECTS        = "Projects";
        public const string TICKETS         = "Tickets";
        public const string TICKET_COMMENTS = "TicketComments";
        public const string ROLES           = "Roles";
    }

    public static class FileType
    {
        public const string BMP  = ".bmp";
        public const string GIF  = ".gif";
        public const string JPG  = ".jpg";
        public const string JPEG = ".jpeg";
        public const string PNG  = ".png";
    }

    public static class PartialViewName
    {
        public const string ADD_COMMENT        = "_AddComment";
        public const string COMMENTS           = "_Comments";
        public const string TICKET_ATTACHMENTS = "_TicketAttachments";
        public const string TICKET_HISTORY     = "_TicketHistory";
    }
}