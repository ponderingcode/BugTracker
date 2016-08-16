namespace BugTracker
{
    public static class Role
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string DEVELOPER = "Developer";
        public const string PROJECT_MANAGER = "ProjectManager";
        public const string SUBMITTER = "Submitter";
    }

    public static class Priority
    {
        public const string LOW = "Low";
        public const string MEDIUM = "Medium";
        public const string HIGH = "High";
    }

    public static class Status
    {
        public const string SUBMITTED = "Submitted";
        public const string QA = "QA";
        public const string PENDING_APPROVAL = "PendingApproval";
        public const string APPROVED = "Approved";
        public const string REJECTED = "Rejected";
        public const string UNASSIGNED = "Unassigned";
    }

    public static class Common
    {
        public const string ID = "Id";
        public const string USER_NAME = "UserName";
        public const string CREATE = "Create";
        public const string DELETE = "Delete";
        public const string DETAILS = "Details";
        public const string EDIT = "Edit";
        public const string INDEX = "Index";
    }

    public static class ControllerName
    {
        public const string ACCOUNT = "Account";
        public const string HOME = "Home";
        public const string MANAGE = "Manage";
        public const string PROJECTS = "Projects";
        public const string TICKETS = "Tickets";
        public const string ROLES = "Roles";
    }
}