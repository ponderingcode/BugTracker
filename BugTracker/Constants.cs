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
    }
}